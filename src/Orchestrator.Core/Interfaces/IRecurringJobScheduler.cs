using System.Linq.Expressions;

namespace Orchestrator.Core.Interfaces;

public interface IRecurringJobScheduler
{
    string CreateRecurringScheduleJob(string ScheduleId, string name, string cronExpression);
}
