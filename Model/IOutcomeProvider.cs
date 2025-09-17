using ExpenseTracker.Model.OutcomeSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    public interface IOutcomeProvider
    {
       List<IOutcome> Outcomes { get; }
    }
}
