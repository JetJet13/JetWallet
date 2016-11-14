using GalaSoft.MvvmLight;
using JetWallet.Model;
using JetWallet.Tools;
using JetWallet.Controller;

namespace JetWallet.ViewModel
{

    public class JetWelcomeViewModel : ViewModelBase
    {

        public string TextTitle
        {
            get { return JetTextTools.RetrieveStringFromResource("Welcome_Title"); }
        }

        public string TextHeader
        {
            get { return JetTextTools.RetrieveStringFromResource("Welcome_Header"); }

        }
        public string TextIntroMessage
        {
            get { return JetTextTools.RetrieveStringFromResource("Welcome_IntroMessage"); }
        }
        public string TextOutroMessage
        {
            get { return JetTextTools.RetrieveStringFromResource("Welcome_OutroMessage"); }
        }
        public string TextQuoteMessage
        {
            get { return JetTextTools.RetrieveStringFromResource("Welcome_QuoteMessage"); }

        }
        public string TextCreateWallet
        {
            get { return JetTextTools.RetrieveStringFromResource("Welcome_CreateWallet"); }

        }
        public string TextSkip
        {
            get { return JetTextTools.RetrieveStringFromResource("Welcome_Skip"); }

        }

        public JetWelcomeViewModel()
        {
            
        }


    }
}