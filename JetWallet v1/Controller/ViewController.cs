using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using JetWallet.View;
using JetWallet.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System.Diagnostics;

namespace JetWallet.Controller
{

    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewController
    {
        public ViewController()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainWindow>();
            SimpleIoc.Default.Register<CreateWalletView>();
            SimpleIoc.Default.Register<PromptPasswordView>();
            SimpleIoc.Default.Register<ManageWalletView>();
            SimpleIoc.Default.Register<WelcomeView>();
            SimpleIoc.Default.Register<SimpleDialogView>();
            SimpleIoc.Default.Register<RecoverWalletView>();
            SimpleIoc.Default.Register<RecoveryPhraseView>();
            SimpleIoc.Default.Register<SetPasswordView>();
            SimpleIoc.Default.Register<SetLanguageView>();
            SimpleIoc.Default.Register<ReceiveView>();
            SimpleIoc.Default.Register<SendView>();
            SimpleIoc.Default.Register<TxInfoView>();
            SimpleIoc.Default.Register<WalletInfoView>();
            SimpleIoc.Default.Register<LicenseView>();
            SimpleIoc.Default.Register<AboutView>();

            SetupViews();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        private IView Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindow>();
            }
        }

        private IView About
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AboutView>();
            }
        }
        private IView CreateWallet
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CreateWalletView>();
            }
        }
        private IView License
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LicenseView>();
            }
        }
        private IView ManageWallet
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ManageWalletView>();
            }
        }
        private IView PromptPassword
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PromptPasswordView>();
            }
        }
        private IView Receive
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ReceiveView>();
            }
        }
        private IView RecoverWallet
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RecoverWalletView>();
            }
        }
        private IView RecoveryPhrase
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RecoveryPhraseView>();
            }
        }
        private IView Send
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SendView>();
            }
        }
        private IView SetLanguage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SetLanguageView>();
            }
        }
        private IView SetPassword
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SetPasswordView>();
            }
        }
        private IView SimpleDialog
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SimpleDialogView>();
            }
        }
        private IView TxInfo
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TxInfoView>();
            }
        }
        private IView WalletInfo
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WalletInfoView>();
            }
        }
        private IView Welcome
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WelcomeView>();
            }
        }
        
        private void ConfigureViews()
        {
            Register(About);
            Register(CreateWallet);
            Register(License);
            Register(ManageWallet);
            Register(PromptPassword);
            Register(Receive);
            Register(RecoverWallet);
            Register(RecoveryPhrase);
            Register(Send);
            Register(SetLanguage);
            Register(SetPassword);
            Register(SimpleDialog);
            Register(TxInfo);
            Register(WalletInfo);
            Register(Welcome);
        }

        private void SetupViews()
        {
            Messenger.Default.Register<string>(this, "OpenAboutView", (string s) => About.ShowDialog());
            Messenger.Default.Register<string>(this, "CloseAboutView", (string s) => About.Close());

            Messenger.Default.Register<string>(this, "OpenCreateWalletView", (string s) => CreateWallet.ShowDialog());
            Messenger.Default.Register<string>(this, "CloseCreateWalletView", (string s) => CreateWallet.Close());

            Messenger.Default.Register<string>(this, "OpenLicenseView", (string s) => License.ShowDialog());
            Messenger.Default.Register<string>(this, "CloseLicenseView", (string s) => License.Close());

            Messenger.Default.Register<string>(this, "OpenManageWalletView", (string s) => ManageWallet.ShowDialog());
            Messenger.Default.Register<string>(this, "CloseManageWalletView", (string s) => ManageWallet.Close());

            Messenger.Default.Register<string>(this, "OpenPromptPasswordView", (string s) => PromptPassword.ShowDialog());
            Messenger.Default.Register<string>(this, "ClosePromptPasswordView", (string s) => PromptPassword.Close());

            Messenger.Default.Register<string>(this, "OpenReceiveView", (string s) => Receive.ShowDialog());
            Messenger.Default.Register<string>(this, "CloseReceiveView", (string s) => Receive.Close());

            Messenger.Default.Register<string>(this, "OpenRecoverWalletView", (string s) => RecoverWallet.ShowDialog());
            Messenger.Default.Register<string>(this, "CloseRecoverWalletView", (string s) => RecoverWallet.Close());

            Messenger.Default.Register<string>(this, "OpenRecoveryPhraseView", (string s) => RecoveryPhrase.ShowDialog());
            Messenger.Default.Register<string>(this, "CloseRecoveryPhraseView", (string s) => RecoveryPhrase.Close());

            Messenger.Default.Register<string>(this, "OpenSendView", (string s) => Send.ShowDialog());
            Messenger.Default.Register<string>(this, "CloseSendView", (string s) => Send.Close());

            Messenger.Default.Register<string>(this, "OpenSetLanguageView", (string s) => SetLanguage.ShowDialog());
            Messenger.Default.Register<string>(this, "CloseSetLanguageView", (string s) => SetLanguage.Close());

            Messenger.Default.Register<string>(this, "OpenSetPasswordView", (string s) => SetPassword.ShowDialog());
            Messenger.Default.Register<string>(this, "CloseSetPasswordView", (string s) => SetPassword.Close());

            Messenger.Default.Register<string>(this, "OpenSimpleDialogView", (string s) => SimpleDialog.ShowDialog());
            Messenger.Default.Register<string>(this, "CloseSimpleDialogView", (string s) => SimpleDialog.Close());

            Messenger.Default.Register<string>(this, "OpenTxInfoView", (string s) => TxInfo.ShowDialog());
            Messenger.Default.Register<string>(this, "CloseTxInfoView", (string s) => TxInfo.Close());

            Messenger.Default.Register<string>(this, "OpenWalletInfoView", (string s) => WalletInfo.ShowDialog());
            Messenger.Default.Register<string>(this, "CloseWalletInfoView", (string s) => WalletInfo.Close());

            Messenger.Default.Register<string>(this, "OpenWelcomeView", (string s) => Welcome.ShowDialog());
            Messenger.Default.Register<string>(this, "CloseWelcomeView", (string s) => Welcome.Close());

        }

        private void Register(IView view)
        {
            string name = view.ToString();
            string open = "Open" + name;
            string close = "Close" + name;
            Messenger.Default.Register<string>(this, open, (string s) => view.ShowDialog());
            Messenger.Default.Register<string>(this, close, (string s) => view.Close());
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }

    }

}
