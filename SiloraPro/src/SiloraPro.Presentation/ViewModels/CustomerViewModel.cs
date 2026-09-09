using System.Windows.Controls;
using SiloraPro.Application.Interfaces;

namespace SiloraPro.Presentation.ViewModels;

/// <summary>
/// ViewModel للعملاء - تطبيق نمط MVVM
/// </summary>
public class CustomerViewModel : ViewModelBase
{
    private readonly ICustomerService _customerService;
    
    public CustomerViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // سيتم إضافة الخصائص والأوامر هنا
}

/// <summary>
/// ViewModel للمنتجات
/// </summary>
public class ProductViewModel : ViewModelBase
{
    private readonly IProductService _productService;
    
    public ProductViewModel(IProductService productService)
    {
        _productService = productService;
    }
}

/// <summary>
/// ViewModel للطلبات
/// </summary>
public class OrderViewModel : ViewModelBase
{
    private readonly IOrderService _orderService;
    
    public OrderViewModel(IOrderService orderService)
    {
        _orderService = orderService;
    }
}

/// <summary>
/// ViewModel الرئيسي
/// </summary>
public class MainViewModel : ViewModelBase
{
    public CustomerViewModel CustomerViewModel { get; }
    public ProductViewModel ProductViewModel { get; }
    public OrderViewModel OrderViewModel { get; }

    public MainViewModel(
        CustomerViewModel customerViewModel,
        ProductViewModel productViewModel,
        OrderViewModel orderViewModel)
    {
        CustomerViewModel = customerViewModel;
        ProductViewModel = productViewModel;
        OrderViewModel = orderViewModel;
    }
}
