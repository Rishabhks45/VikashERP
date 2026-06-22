using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using MobileApp.Services;

namespace MobileApp.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _hasError;

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set 
        {
            SetProperty(ref _errorMessage, value);
            HasError = !string.IsNullOrEmpty(value);
        }
    }

    public bool HasError
    {
        get => _hasError;
        set => SetProperty(ref _hasError, value);
    }

    public Command LoginCommand { get; }

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
        Title = "Login";
        LoginCommand = new Command(async () => await ExecuteLoginCommand(), () => !IsBusy);
    }

    private async Task ExecuteLoginCommand()
    {
        if (IsBusy) return;

        // Basic validation
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            await Shell.Current.DisplayAlertAsync("Validation", "Email and Password are required.", "OK");
            return;
        }

        IsBusy = true;
        ErrorMessage = string.Empty;
        LoginCommand.ChangeCanExecute();

        try
        {
            bool success = await _authService.LoginAsync(Email.Trim(), Password);
            if (success)
            {
                // Successful login. Navigate to main shell route.
                // We use "///MainPage" to replace the navigation stack.
                await Shell.Current.GoToAsync("///MainPage");
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Login Failed", "Invalid email or password.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"An unexpected error occurred: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            LoginCommand.ChangeCanExecute();
        }
    }
}
