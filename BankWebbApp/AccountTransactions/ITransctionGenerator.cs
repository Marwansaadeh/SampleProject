namespace BankWebbApp.AccountTransactions
{
    public interface ITransctionGenerator
    {
        bool ValidateOperation();
        void SaveTransaction();
    }
}
