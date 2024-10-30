using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using SmsCenter.UI.Shared.Services;
using SmsCenter.UI.Shared.ViewModels;

namespace SmsCenter.UI.Shared.Controls
{
    public class MenuItemViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public ICommand MenuCommand { get; }
        public ViewType Type { get; set; }
        public PackIconKind Icon { get; set; }
        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnPropertyChanged();
            }
        }
        public string Name { get; set; }

        public MenuItemViewModel(INavigationService navigationService,
            IEventNotificationService eventNotificationService)
        {
            MenuCommand = new MenuCommand(this, navigationService, eventNotificationService);
        }
    }

    public class MenuViewModel : ViewModelBase
    {
        private readonly IEventNotificationService _service;

        public List<MenuItemViewModel> MenuItems { get; set; } = [];

        public MenuViewModel(IEventNotificationService service)
        {
            _service = service;
            _service.ActiveMenuItemChanged += OnActiveMenuItemChanged;
        }

        private void OnActiveMenuItemChanged(int id)
        {
            var oldButton = MenuItems.FirstOrDefault(x => x.IsActive);
            var newButton = MenuItems.FirstOrDefault(x => x.Id == id);
        
            if (oldButton != null && newButton != null)
            {
                oldButton.IsActive = false;
                newButton.IsActive = true;
            }
        
            OnPropertyChanged(nameof(MenuItems));
        }
    }
}