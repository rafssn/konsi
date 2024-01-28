namespace konsi_api.Models.Events
{
    public class CpfSearchedEvent
    {
        public CpfSearchedEvent(string cpf) {

            if (string.IsNullOrWhiteSpace(cpf))
                throw new NotSupportedException();

            this.Cpf = cpf;
        }

        public string Cpf { get; set; }
    }
}
