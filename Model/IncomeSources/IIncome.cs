using ExpenseTracker.Model.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.IncomeSources
{
    public interface IIncome
    {
        string IncomeId { get; }
        string IncomeName { get; }
        float Amount { get; }
        SourceOfIncome SourceOfIncome { get; }
        bool Freeze { get; }
        void CreateIncome(string incomeName, SourceOfIncome sourceOfIncome);
        bool UpdateAmount(float newAmount);
        void UpdateFreezingStatus(bool status);
    }
}
