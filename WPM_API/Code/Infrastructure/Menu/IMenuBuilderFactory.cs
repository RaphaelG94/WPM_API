namespace WPM_API.Code.Infrastructure.Menu
{
    public interface IMenuBuilderFactory
    {
        IMenuBuilder Create<T>() where T : MenuBuilderBase;
    }
}
