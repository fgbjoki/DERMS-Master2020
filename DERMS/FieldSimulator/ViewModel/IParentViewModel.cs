namespace FieldSimulator.ViewModel
{
    internal interface IParentViewModel
    {
        BaseViewModel ChildViewModel { get; set; }
    }
}
