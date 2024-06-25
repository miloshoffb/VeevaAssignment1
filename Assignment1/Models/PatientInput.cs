using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Assignment1.Models
{
    public class PatientInput
    {
        [DisplayName("Height (cm)")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Height must be greater than or equal to 0.")]
        public double HeightCm { get; set; }

        [DisplayName("Weight (kg)")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Weight must be greater than or equal to 0.")]
        public double WeightKg { get; set; }

        [DisplayName("Gender")]
        public string Gender { get; set; }

        [DisplayName("Age")]
        [DataType(DataType.Text)]
        [Range(0, int.MaxValue, ErrorMessage = "Age must be greater than or equal to 0.")]
        public byte Age { get; set; }

        [DisplayName("BSA Formula")]
        public BsaFormula BsaFormula { get; set; }

        public double CalculatedBsa { get; set; } 
    }
}