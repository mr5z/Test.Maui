using Microsoft.Extensions.Logging;
using PropertyValidator.Exceptions;
using PropertyValidator.Models;
using PropertyValidator.Services;
using PropertyValidator.ValidationPack;
using System.Windows.Input;
using Test.Maui.Models;
using Test.Maui.Rules;
using Test.Maui.Services;

namespace Test.Maui.ViewModels;

internal class MainPageViewModel : BaseViewModel, INotifiableModel
{
    private readonly IValidationService validationService;
    private readonly IToastService toastService;
    private readonly ILogger<MainPageViewModel> logger;

    public MainPageViewModel(
        IValidationService validationService,
        IToastService toastService,
        ILogger<MainPageViewModel> logger)
    {
        this.validationService = validationService;
        this.toastService = toastService;
        this.logger = logger;

        SubmitCommand = new DelegateCommand(Submit);
    }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EmailAddress { get; set; }
    public Address PhysicalAddress { get; set; } = new Address();
    public ICommand SubmitCommand { get; set; }

    public IDictionary<string, string?> Errors => validationService.GetErrors();

    public override void Initialize(INavigationParameters parameters)
    {
        validationService.For(this,
            delay: TimeSpan.FromSeconds(0.7))
            .AddRule(e => e.FirstName, new StringRequiredRule(), new MinLengthRule(2))
            .AddRule(e => e.LastName, new StringRequiredRule(), new MaxLengthRule(5))
            .AddRule(e => e.EmailAddress, new StringRequiredRule(), new EmailFormatRule(), new RangeLengthRule(10, 15))
            .AddRule(e => e.PhysicalAddress, new AddressRule());
    }

    public void NotifyErrorPropertyChanged() => RaisePropertyChanged(nameof(Errors));

    private void Submit()
    {
        try
        {
            logger.LogInformation("LastName: {LastName}", LastName);
            validationService.EnsurePropertiesAreValid();
        }
        catch (PropertyException ex)
        {
            var resultArgs = ex.ValidationResultArgs;
            var errors = string.Join(", ", resultArgs.ErrorMessages);
            toastService.ShowMessage("Errors: {0}", errors);
        }
    }
}
