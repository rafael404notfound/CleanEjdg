using CleanEjdg.Core.Application.Common;
using CleanEjdg.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CleanEjdg.Core.Application.Repositories
{
    public class EFCatRepository : IRepositoryBase<Cat>
    {
        IApplicationDbContext DbContext;
        public EFCatRepository(IApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task Create(Cat entity)
        {
            /*
            // Create a new cat instance that will be saved in db
            Cat newCat = new Cat()
            {
                Name = entity.Name,
                DateOfBirth = entity.DateOfBirth,
                IsVaccinated = entity.IsVaccinated,
                HasChip = entity.HasChip,
                IsSterilized = entity.IsSterilized,
            };

            // Create a CatPhoto list to store the upload files
            List<CatPhoto> photoList = new List<CatPhoto>();
            if (entity.Files != null)
            {
                if (entity.Files?.Count > 0)
                {
                    foreach(var browserFile in entity.Files)
                    {
                        if (browserFile.Size > 0)
                        {
                            
                                var Stream = browserFile.OpenReadStream();
                                using (var MemoryStream = new MemoryStream())
                                {
                                    Stream.CopyTo(MemoryStream);
                                    var newPhoto = new CatPhoto()
                                    {
                                        Bytes = MemoryStream.ToArray(),
                                        Description = browserFile.Name,
                                        FileExtension = browserFile.ContentType,
                                        Size = browserFile.Size,
                                    };
                                    // Add the photo instance to the list
                                    photoList.Add(newPhoto);
                                }
                        }
                    }
                }
            }
            // Assign the photos to the Cat
            newCat.Photos = photoList;
            */
            /////////////////////////////////////////////////////
            /*if (entity.Files != null)
            {
                foreach (var file in entity.Files)
                {
                    if (file.Size > 0)
                    {
                        var Stream = file.OpenReadStream();
                        using (var MemoryStream = new MemoryStream())
                        {
                            await Stream.CopyToAsync(MemoryStream);
                            var newPhoto = new CatPhoto()
                            {
                                Bytes = MemoryStream.ToArray(),
                                Description = file.Name,
                                FileExtension = file.ContentType,
                                Size = file.Size,
                            };
                            // Add the photo instance to the list
                            entity.Photos.Add(newPhoto);
                        }
                    }
                }
            }*/
            
            // Save new Cat in db
            await DbContext.Cats.AddAsync(entity);
            await DbContext.SaveChangesAsync();
        }

        // Returns false if an the entity didnt exist
        public async Task Delete(int id)
        {
            DbContext.Cats.Remove(new Cat { Id = id });
            await DbContext.SaveChangesAsync();
        }

        public async Task<Cat> Get(int id)
        {
            return await DbContext.Cats.Include(c => c.Photos).FirstOrDefaultAsync(c => c.Id == id) ?? new Cat();
        }

        public IQueryable<Cat> GetAll()
        {
            return DbContext.Cats.Include(c => c.Photos);
        }

        public async Task Update(Cat entity)
        {
            // entity as it currently exists in the db
            var cat = DbContext.Cats.Include(c => c.Photos)
                .FirstOrDefault(p => p.Id == entity.Id);
            // update properties on the parent
            DbContext.Entry(cat).CurrentValues.SetValues(entity);
            // remove or update child collection items
            var catPhotos = cat.Photos;
            foreach (var catPhoto in catPhotos)
            {
                var photo = entity.Photos.SingleOrDefault(p => p.Id == catPhoto.Id);
                if (photo != null)
                {
                    //no need to change the photo since photos never change, htey are only added or removed
                    //DbContext.Entry(catPhotos).CurrentValues.SetValues(photo);
                }
                else
                    DbContext.Remove(catPhoto);
            }
            // add the new items
            foreach (var photo in entity.Photos)
            {
                if (catPhotos.All(i => i.Id != photo.Id))
                {
                    cat.Photos.Add(photo);
                }
            }
            /*var dbEntity = DbContext.Cats.Include(c => c.Photos).Single(c => c.Id == entity.Id);
            DbContext.Entry(dbEntity).CurrentValues.SetValues(entity);

            if (dbEntity.Photos != null)
            {
                if (entity.Photos != null)
                {
                    foreach(var photo in dbEntity.Photos)
                    {
                        if(entity.Photos.FirstOrDefault(p => p.Id == photo.Id) != null)
                        {
                            DbContext.Entry(dbEntity.Photos.Single(p => p.Id == photo.Id)).CurrentValues.SetValues(entity.Photos.Single(p => p.Id == photo.Id));
                        }
                        else
                        {
                            // Remove deleted image
                            context.SubFoos.Attach(newFoo.SubFoo);
                            dbFoo.SubFoo = newFoo.SubFoo;
                        }
                    }
                }
                else // relationship has been removed
                    dbFoo.SubFoo = null;
            }
            else
            {
                if (newFoo.SubFoo != null) // relationship has been added
                {
                    // Attach assumes that newFoo.SubFoo is an existing entity
                    context.SubFoos.Attach(newFoo.SubFoo);
                    dbFoo.SubFoo = newFoo.SubFoo;
                }
                // else -> old and new SubFoo is null -> nothing to do
            }*/
            await DbContext.SaveChangesAsync();
        }
    }
}
