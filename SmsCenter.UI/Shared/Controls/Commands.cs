using SmsCenter.UI.Shared.Commands;
using SmsCenter.UI.Shared.Notifications;
using SmsCenter.UI.Shared.Services;

namespace SmsCenter.UI.Shared.Controls
{
    public class MenuCommand : CommandBase
    {
        private readonly MenuItemViewModel _viewModel;
        private readonly INavigationService _service;
        private readonly IEventNotificationService _eventService;

        public MenuCommand(MenuItemViewModel viewModel, INavigationService service,
            IEventNotificationService eventService)
        {
            _viewModel = viewModel;
            _service = service;
            _eventService = eventService;
        }

        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            try
            {
                new NavigateCommand(_service).Execute(_viewModel.Type);
                _viewModel.IsActive = true;
                _eventService.SetActiveMenuItem(_viewModel.Id);
            }
            catch (Exception e)
            {
                PopupNotification.ShowError("Не удалось загрузить страницу", e.Message);
            }
        }
    }
}