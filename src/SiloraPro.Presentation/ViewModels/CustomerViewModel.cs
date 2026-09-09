using SiloraPro.Application.Interfaces;
using SiloraPro.Domain.Entities;

namespace SiloraPro.Presentation.ViewModels;

public class CustomerViewModel : ViewModelBase
{
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;

    private List<Customer> _customers = new();
    private Customer? _selectedCustomer;

    public List<Customer> Customers
    {
        get => _customers;
        set => SetProperty(ref _customers, value);
    }

    public Customer? SelectedCustomer
    {
        get => _selectedCustomer;
        set => SetProperty(ref _selectedCustomer, value);
    }

    public CustomerViewModel(ICustomerService customerService, IProductService productService, IOrderService orderService)
    {
        _customerService = customerService;
        _productService = productService;
        _orderService = orderService;
        
        LoadCustomers();
    }

    private async void LoadCustomers()
    {
        Customers = await _customerService.GetAllAsync();
    }
}
