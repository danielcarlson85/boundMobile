namespace Bound.Tablet.ViewModels
{
    public class DonePageViewModel : BaseViewModel
    {

        string titleText;
        public string TitleText
        {
            get { return titleText; }
            set { SetProperty(ref titleText, value); }
        }

        public DonePageViewModel()
        {
            TitleText = "Press the button below when you are finished with your workout";
        }
    }
}