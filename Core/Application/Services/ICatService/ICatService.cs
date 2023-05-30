namespace CleanEjdg.Core.Application.Services {

    public interface ICatService {
        Dictionary<string, int> CalculateCatAge(Cat cat);
    }    
}