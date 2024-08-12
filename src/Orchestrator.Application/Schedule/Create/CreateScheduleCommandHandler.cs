using MediatR;
using SharedUtilLibrary;
using NCrontab;
using Orchestrator.Core.Interfaces;

namespace Orchestrator.Application.Schedule.Create;

public class CreateScheduleCommandHandler(IMongoRepository<Domain.Entities.Schedule> scheduleRepository, IRecurringJobScheduler recurringJobScheduler)
    : IRequestHandler<CreateScheduleCommandRequest, string>
{
    public async Task<string> Handle(CreateScheduleCommandRequest request, CancellationToken cancellationToken)
    {
        var schedule = await scheduleRepository.FindOne(x => x.Name == request.Name);
        if (schedule != null)
        {
            throw new ApplicationException("Schedule name already exists.");
        }

        schedule = new Domain.Entities.Schedule
        {
            Name = request.Name,
            Description = request.Description,
            CronExpression = request.CronExpression,
            IsActive = request.IsActive,
            ScheduleId = Guid.NewGuid().ToString(),
            Topic = request.Topic,
            ProcessCode = request.ProcessCode,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            Priority = request.Priority
        };

        await scheduleRepository.InsertOne(schedule);

        recurringJobScheduler.CreateRecurringScheduleJob(schedule.ScheduleId, schedule.Name, request.CronExpression);

        return schedule.ScheduleId;
    }
}

public class CreateScheduleCommandRequest : IRequest<string>
{
    private string _cronExpression;
    public required string CronExpression
    {
        get => _cronExpression;
        set
        {
            try
            {
                CrontabSchedule.Parse(value);
                _cronExpression = value;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Invalid cron expression.", ex);
            }
        }
    }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public required string Topic { get; set; }
    public required string ProcessCode { get; set; }
    public int Priority { get; set; }
}
