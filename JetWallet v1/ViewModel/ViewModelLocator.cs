/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:JetWallet.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using JetWallet.Model;

namespace JetWallet.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
                       

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<CreateWalletViewModel>(true);
            SimpleIoc.Default.Register<PromptPasswordViewModel>(true);
            SimpleIoc.Default.Register<ManageWalletViewModel>(true);
            SimpleIoc.Default.Register<WelcomeViewModel>(true);
            SimpleIoc.Default.Register<SimpleDialogViewModel>(true);
            SimpleIoc.Default.Register<RecoverWalletViewModel>(true);
            SimpleIoc.Default.Register<RecoveryPhraseViewModel>(true);
            SimpleIoc.Default.Register<SetPasswordViewModel>(true);
            SimpleIoc.Default.Register<ColorSchemeViewModel>(true);
            SimpleIoc.Default.Register<SetLanguageViewModel>(true);
            SimpleIoc.Default.Register<ReceiveViewModel>(true);
            SimpleIoc.Default.Register<SendViewModel>(true);
            SimpleIoc.Default.Register<TxInfoViewModel>(true);
            SimpleIoc.Default.Register<CurrencyViewModel>(true);
            SimpleIoc.Default.Register<FeeViewModel>(true);
            SimpleIoc.Default.Register<WalletInfoViewModel>(true);
            SimpleIoc.Default.Register<LanguageViewModel>(true);
            SimpleIoc.Default.Register<LicenseViewModel>(true);
            SimpleIoc.Default.Register<AboutViewModel>(true);
            SimpleIoc.Default.Register<UiSettings>(true);

        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public CreateWalletViewModel CreateWallet
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CreateWalletViewModel>();
            }
        }

        public PromptPasswordViewModel PromptPassword
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PromptPasswordViewModel>();
            }
        }

        public ManageWalletViewModel ManageWallet
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ManageWalletViewModel>();
            }
        }

        public WelcomeViewModel Welcome
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WelcomeViewModel>();
            }
        }

        public SimpleDialogViewModel SimpleDialog
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SimpleDialogViewModel>();
            }
        }

        public RecoverWalletViewModel RecoverWallet
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RecoverWalletViewModel>();
            }
        }

        public RecoveryPhraseViewModel RecoveryPhrase
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RecoveryPhraseViewModel>();
            }
        }

        public SetPasswordViewModel SetPassword
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SetPasswordViewModel>();
            }
        }

        public ColorSchemeViewModel ColorScheme
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ColorSchemeViewModel>();
            }
        }

        public SetLanguageViewModel SetLanguage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SetLanguageViewModel>();
            }
        }

        public ReceiveViewModel Receive
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ReceiveViewModel>();
            }
        }

        public SendViewModel Send
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SendViewModel>();
            }
        }

        public TxInfoViewModel TxInfo
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TxInfoViewModel>();
            }
        }

        public CurrencyViewModel Currency
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CurrencyViewModel>();
            }
        }

        public FeeViewModel Fee
        {
            get
            {
                return ServiceLocator.Current.GetInstance<FeeViewModel>();
            }
        }

        public WalletInfoViewModel WalletInfo
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WalletInfoViewModel>();
            }
        }

        public LanguageViewModel Language
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LanguageViewModel>();
            }
        }

        public LicenseViewModel License
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LicenseViewModel>();
            }
        }

        public AboutViewModel About
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AboutViewModel>();
            }
        }

        public UiSettings UiSettings
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UiSettings>();
            }
        }


        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}