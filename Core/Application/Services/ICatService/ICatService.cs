namespace CleanEjdg.Core.Application.Services {

    public interface ICatService {
        Dictionary<string, int> CatAge(Cat cat);
    }    
}