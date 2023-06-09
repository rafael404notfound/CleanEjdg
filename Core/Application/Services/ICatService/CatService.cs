using CleanEjdg.Core.Application.Repositories;

namespace CleanEjdg.Core.Application.Services {

    public class CatService : ICatService {
        
        IDateTimeServer DateTimeServer;
        public CatService(IDateTimeServer dateTimeServer)
        {
            DateTimeServer = dateTimeServer;
        }

        /*
         *  Method CalculateCatAge returns a dictionary with the following values:

         *  Keys:  
         *      dictionary["Years"] = number of complete years that have passed since date of birth
         *      dictionary["Months"] = number of complete months that have passed since last completed year

         *  Example: 
         *      If Cat.DateOfBirth was 4 years and 11 months ago:
         *          dictionary["Years"] value is 4
         *          dictionary["Years"] value is 11
        */
        public Dictionary<string, int> CalculateCatAge(Cat cat){
            Dictionary<string, int> result = new Dictionary<string, int>();  
            
            result["Months"] = DateTimeServer.Now.Month - cat.DateOfBirth.Month;
            
            if(result["Months"] < 0) {
                result["Years"] = DateTimeServer.Now.Year - cat.DateOfBirth.Year - 1;
                result["Months"] = 12 + result["Months"];
            } else {
                result["Years"] = DateTimeServer.Now.Year - cat.DateOfBirth.Year;
            }      
            
            return result;
        }
    }
}