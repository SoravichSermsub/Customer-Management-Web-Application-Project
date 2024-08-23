using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebAppProject.Models
{
    public class CustomerCompany
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        public string Company_Name { get; set;}

        [Required]
        public string Branch { get; set;}

        [Required]
        public string Nationality { get; set; }

        [Required]
        public string Key_Person { get; set;}

        [Required]
        public string Key_Person_Phone { get; set; }

        [Required]
        public string Website { get; set;}

        [Required]
        public string ProductService { get; set;}

        [Required]
        public string Type_of_Business { get; set;}

        [Required]
        public int Number_of_employees { get; set; }

        [Required]
        public string Fax { get; set;}

        [Required]
        public string Authorized_person_contract_Phone { get; set;}

        [Required]
        public string Email{ get; set;}

        [Required]
        public string Date_of_Establishment { get; set;}

        [Required]
        public string Address { get; set;}

        [Required]
        public string JCC_Member { get; set; }
    }
}