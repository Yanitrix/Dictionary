namespace Domain.Commands
{
    //T si command type, R is command return type
    public interface ICommandHandler<in T, R> where T : ICommand
    {
        Response<R> Handle(T command);
    }
}