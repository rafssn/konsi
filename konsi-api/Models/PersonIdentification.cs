using System.Text.RegularExpressions;

namespace konsi_api.Models
{
    public class PersonIdentification
    {
        public PersonIdentification(string cpf)
        {
            Cpf = cpf;
            NonMaskedCpf = Regex.Replace(this.Cpf, "[^0 - 9.]", "", RegexOptions.IgnoreCase);
        }

        private string Cpf { get; set; }
        private string NonMaskedCpf { get; set; }

        public bool IsValid()
        {
            return this.NonMaskedCpf?.Length == 11;
        }

        public string GetCpf()
        {
            return this.NonMaskedCpf;
        }
    }
}
