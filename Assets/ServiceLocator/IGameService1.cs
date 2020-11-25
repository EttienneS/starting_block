namespace Assets.ServiceLocator
{
    public interface IGameService
    {
        void BindServiceLocator(ServiceLocator locator);

        void Initialize();
    }
}