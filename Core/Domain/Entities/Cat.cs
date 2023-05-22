using CleanEjdg.Core.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace CleanEjdg.Core.Domain.Entities {

    public class Cat : BaseEntity {
        public string? Name { get; set; } = string.Empty;
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public bool IsVaccinated { get; set; } = false;
        [Required]
        public bool HasChip { get; set; } = false;
        [Required]
        public bool IsSterilized { get; set; } = false;


        /* *****NO LONGER NEEDED BEACAUSE AGE IS NOW CALCULATED THROUGH CATSERVICE*****

        //Age explanation:

        //  Keys:  
        //      Age["Years"] = number of complete years that have passed since date of birth
        //      Age["Months"] = number of complete months that have passed since last completed year

        //  Example: 
        //      If DateOfBirth was 4 years and 11 months ago:
        //          Age["Years"] value is 4
        //          Age["Years"] value is 11
        public IDictionary<string, int> Age { 
            get { return CalculateAge(); }
            set { Age = value; }
        }

        private Dictionary<string, int> CalculateAge() {
            Dictionary<string, int> result = new Dictionary<string, int>();  
            
            result["Months"] = DateTime.Now.Month - DateOfBirth.Month;
            
            if(result["Months"] < 0) {
                result["Years"] = DateTime.Now.Year - DateOfBirth.Year - 1;
                result["Months"] = 12 + result["Months"];
            } else {
                result["Years"] = DateTime.Now.Year - DateOfBirth.Year;
            }      
            
            return result;
        }*/
    }
}