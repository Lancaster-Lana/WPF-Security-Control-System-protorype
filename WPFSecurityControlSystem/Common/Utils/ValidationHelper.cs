using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.ComponentModel;
using System.Reflection;
using System.Net;
using WPFSecurityControlSystem.Base;
using Microsoft.Windows.Controls;

namespace WPFSecurityControlSystem.Utils
{
    public enum ValidationFormat { Required, Numeric, Date, IPAddress, Range}


    #region Validators - ValidationRules

    public class TypeValidator<T> : ValidationRule where T : IConvertible
    {
        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                if (string.IsNullOrEmpty(_errorMessage))
                    _errorMessage = "Wrong value. Please, check the value to be compatible with required type.";//"Value is not a type of " + typeof(T).Name;
                return _errorMessage;
            }
            set { _errorMessage = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);

            try //try to convert to type
            {
                //1.DateTime parsing 
                if (value is DateTime && value is T)
                    return result; 

                //2. other types
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));                
                T realObj = (T)converter.ConvertFrom(value);           
            }
            catch
            {
                result = new ValidationResult(false, this.ErrorMessage);
            }
            return result;
        }

    }

    public class StringValidator: ValidationRule
    {
        private int _minimumLength = -1;
        private int _maximumLength = -1;
       

        public int MinimumLength
        {
            get { return _minimumLength; }
            set { _minimumLength = value; }
        }

        public int MaximumLength
        {
            get { return _maximumLength; }
            set { _maximumLength = value; }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                if (string.IsNullOrEmpty(_errorMessage))
                    _errorMessage = "Value is required.";
                return _errorMessage; 
            }
            set { _errorMessage = value; }
        }

        public StringValidator()
        { 
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);
            string inputString = (value ?? string.Empty).ToString();
            if (inputString.Length < this.MinimumLength || (this.MaximumLength > 0 && inputString.Length > this.MaximumLength))
            {
                result = new ValidationResult(false, this.ErrorMessage);
            }
            return result;
        }

    }

    public class IPValidator : ValidationRule
    {        
        public string ErrorMessage
        {
            get 
            {
                return "Wrong IP address. Please enter [0..255].[0..255].[0..255].[0..255]"; 
            }            
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            IPAddress ipaddress = null;
            ValidationResult result = null;
            string ipString = Convert.ToString(value).Replace("_", " ").Replace(",", ".");
            if (ConverterHelper.TryParseIP(ipString, out ipaddress))
                 result = new ValidationResult(true, null);
            else
                 result = new ValidationResult(false, this.ErrorMessage);
            return result;
        }
    }

    public class RangeValidator<T> : ValidationRule where T : IConvertible
    {
        private T _minimum;
        public T Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }

        private T _maximum;
        public T Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                if (string.IsNullOrEmpty(_errorMessage))
                    _errorMessage = "Value must be between " + Minimum + "  and " + Maximum;
                return _errorMessage; 
            }
            set { _errorMessage = value; }
        }

        public RangeValidator(object start, object end)
        {            
            //DateTimeConverter d; d.ConvertFrom(start);
            Minimum = (T)start;//decimal.Parse(Convert.ToString(start));
            Maximum = (T)end;           
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);
            string inputStr = Convert.ToString(value);

            if(String.IsNullOrEmpty(inputStr))
                 result = new ValidationResult(false, this.ErrorMessage);
            else if (typeof(T).Equals(typeof(DateTime)))
            {                
                DateTime input;
                DateTime min;
                DateTime max;
                if (!DateTime.TryParse(inputStr, out input))
                    result = new ValidationResult(false, "Input value '" + inputStr + "' is not a Date format");
                else if (!DateTime.TryParse(Minimum.ToString(), out min))
                   result = new ValidationResult(false, "Minimum value '" + Minimum + "' is not a Date format");
                else if (!DateTime.TryParse(Maximum.ToString(), out max))
                    result = new ValidationResult(false, "Maximum value  '" + Maximum + "' has a Date  format");
                else if(input < min || input > max)
                    result = new ValidationResult(false, this.ErrorMessage);               
            }
            else
            {
                decimal input=-1;
                decimal min=-1;
                decimal max=-1;
                if (!decimal.TryParse(inputStr, out input))
                    result = new ValidationResult(false, "Input value has a wrong format");
                else if (!decimal.TryParse(Minimum.ToString(), out min))
                   result = new ValidationResult(false, "Minimum value has a wrong  format");
                else if (!decimal.TryParse(Maximum.ToString(), out max))
                    result = new ValidationResult(false, "Maximum value has a wrong  format");
                else if(input < min || input > max)
                    result = new ValidationResult(false, this.ErrorMessage);
            }
            return result;
        }
    }

    #endregion

    #region Error\Validation Provider

    public class ErrorProvider : Decorator
    {
        private delegate void FoundBindingCallbackDelegate(FrameworkElement element, Binding binding, DependencyProperty dp);
   
        #region Static Members \Methods

        /// <summary>
        /// Register required validator
        /// </summary>
        /// <param name="ctrlToBeValidated"></param>
        public static void RegisterValidator(Control ctrlToBeValidated)
        {
            RegisterValidator(ctrlToBeValidated, ValidationFormat.Required);
        }

        /// <summary>
        /// Register custom validator by custom validation rule
        /// </summary>
        /// <param name="ctrlToBeValidated"></param>
        /// <param name="validationRule"></param>
        /// <param name="propertyToBeValidated"></param>
        public static void RegisterValidator(Control ctrlToBeValidated, ValidationRule validationRule, DependencyProperty propertyToBeValidated)
        {
            //SET bindings dynamically           
            Binding validationBinding = new Binding {  Mode= BindingMode.TwoWay,
                                                        Source = ctrlToBeValidated,
                                                        Path = new PropertyPath("Tag") };
            validationBinding.NotifyOnValidationError = true;
            //validationBinding.ElementName = ctrlToBeValidated.Name;
            validationBinding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            validationBinding.ValidationRules.Add(validationRule);
            BindingOperations.SetBinding(ctrlToBeValidated, propertyToBeValidated, validationBinding);            
        }

        /// <summary>
        /// Temporary method- should be replaced later with <T
        /// </summary>
        /// <param name="ctrlToBeValidated"></param>
        /// <param name="validationType"></param>
        /// <param name="conditions"></param>
        public static void RegisterValidator(Control ctrlToBeValidated, ValidationFormat validationType, params object[] conditions)
        {
            ValidationRule validationRule = null;

            DependencyProperty propertyToBeValidated = GetDependencyPropertyFrom(ctrlToBeValidated);
            switch (validationType)
            {
                case ValidationFormat.Date:
                    validationRule = new TypeValidator<DateTime>();                   
                    //dateValidator.MinimumLength = 1;
                    //validationRule = dateValidator;
                    break;
                case ValidationFormat.Numeric:
                    validationRule = new TypeValidator<Decimal>();
                    //dateValidator.MinimumLength = 1;
                    //validationRule = dateValidator;
                    break;
                case ValidationFormat.Range:
                    if (conditions != null && conditions.Count() > 1)
                        validationRule = new RangeValidator<int>(conditions[0], conditions[1]);
                    break;

                case ValidationFormat.Required:
                    StringValidator reqValidator = new StringValidator();
                    reqValidator.MinimumLength = 1;
                    validationRule = reqValidator;
                    break;

                case ValidationFormat.IPAddress:
                    validationRule = new IPValidator();                   
                    break;

            }
            RegisterValidator(ctrlToBeValidated, validationRule, propertyToBeValidated);
        }

        public static void RegisterRangeValidator<T>(Control ctrlToBeValidated, object min, object max) where T : IConvertible
        {
            RangeValidator<T> validationRule = new RangeValidator<T>(min, max);
            RegisterValidator(ctrlToBeValidated, validationRule, TextBox.TextProperty);
        }

        public static void RegisterValidator<T>(Control ctrlToBeValidated, params object[] conditions) where T : IConvertible
        {
            TypeValidator<T> validationRule = new TypeValidator<T>();
            DependencyProperty dp = GetDependencyPropertyFrom(ctrlToBeValidated);
            RegisterValidator(ctrlToBeValidated, validationRule, dp);
        }

        public static void UnregisterValidator(Control controlToValidate)
        {
            DependencyProperty dp = GetDependencyPropertyFrom(controlToValidate);

            controlToValidate.ClearValue(dp);           
        }

        public static DependencyProperty GetDependencyPropertyFrom(Control ctrlToBeValidated)
        {
            DependencyProperty propertyToBeValidated = TextBox.TextProperty;
            if (ctrlToBeValidated is ComboBox)
                propertyToBeValidated = ComboBox.SelectedValueProperty;
            else if (ctrlToBeValidated is DatePicker)
                propertyToBeValidated = DatePicker.SelectedDateProperty;
            else if (ctrlToBeValidated is DateTimePicker)
                propertyToBeValidated = DateTimePicker.ValueProperty;
            return propertyToBeValidated;
        }

        public static bool Validate(DependencyObject controlToValidate)
        {
            bool isValid = true;
            FrameworkElement firstInvalidElement = null;
         
            //if (this.DataContext is IDataErrorInfo)
            {
                List<Binding> allKnownBindings = ClearInternal(controlToValidate);

                var parent = ((FrameworkElement)controlToValidate);
                // Now show all errors
                foreach (Binding knownBinding in allKnownBindings)
                {                  
                    //string errorMessage = ((IDataErrorInfo)this.DataContext)[knownBinding.Path.Path];
                    //if (errorMessage != null && errorMessage.Length > 0)
                    {                       
                        // Display the error on any elements bound to the property
                        FindBindingsRecursively(controlToValidate,
                        delegate(FrameworkElement element, Binding binding, DependencyProperty dp)
                        {
                            var ctrToValidate = binding.Source as FrameworkElement;
                           
                            if ((element.IsEnabled && element.Visibility == Visibility.Visible)
                                      && binding.Source == knownBinding.Source//element.Name
                                      &&binding.ValidationRules != null && binding.ValidationRules.Count > 0)
                                      //&& knownBinding.Path.Path == binding.Path.Path                             
                            {
                                foreach (ValidationRule rule in knownBinding.ValidationRules)
                                {
                                    // ((binding.ValidationRules != null && binding.ValidationRules.Count > 0)&& 
                                    ValidationResult result = rule.Validate(ctrToValidate.GetValue(dp), CultureInfo.CurrentCulture);
                                    if (!result.IsValid)
                                    {
                                        isValid = false;
                                        string errorMsg = result.ErrorContent.ToString();
                                        BindingExpression expression = element.GetBindingExpression(dp);
                                        ValidationError error = new ValidationError(new ExceptionValidationRule(), expression, errorMsg, null);
                                        Validation.MarkInvalid(expression, error);

                                        if (firstInvalidElement == null)
                                            firstInvalidElement = element;
                                    }
                                }
                            }
                           
                                
                            return;

                        });
                    }
                }
            }
            return isValid;
        }

        /// <summary>
        /// Clears any error messages and returns a list of all bindings on the current form/page. This is simply so 
        /// it can be reused by the Validate method.
        /// </summary>
        /// <returns>A list of all known bindings.</returns>
        private static List<Binding> ClearInternal(DependencyObject parent)
        {
            // Clear all errors
            List<Binding> bindings = new List<Binding>();
            FindBindingsRecursively(parent,  delegate(FrameworkElement element, Binding binding, DependencyProperty dp)
                                                    {
                                                        // Remember this bound element. We'll use this to display error messages for each property.
                                                        bindings.Add(binding);
                                                    }
                                   );
            return bindings;
        }

        /// <summary>
        /// Recursively goes through the control tree, looking for bindings on the current data context.
        /// </summary>
        /// <param name="parent">The root element to start searching at.</param>
        /// <param name="callbackDelegate">A delegate called when a binding if found.</param>
        private static void FindBindingsRecursively(DependencyObject parent, FoundBindingCallbackDelegate callbackDelegate)
        {
            // See if we should display the errors on this element
            MemberInfo[] members = parent.GetType().GetMembers(BindingFlags.Static |
                    BindingFlags.Public |
                    BindingFlags.FlattenHierarchy);

            foreach (MemberInfo member in members)
            {
                DependencyProperty dp = null;

                // Check to see if the field or property we were given is a dependency property
                if (member.MemberType == MemberTypes.Field)
                {
                    FieldInfo field = (FieldInfo)member;
                    if (typeof(DependencyProperty).IsAssignableFrom(field.FieldType))
                    {
                        dp = (DependencyProperty)field.GetValue(parent);
                    }
                }
                else if (member.MemberType == MemberTypes.Property)
                {
                    PropertyInfo prop = (PropertyInfo)member;
                    if (typeof(DependencyProperty).IsAssignableFrom(prop.PropertyType))
                    {
                        dp = (DependencyProperty)prop.GetValue(parent, null);
                    }
                }
              

                if (dp != null)
                {
                    // Awesome, we have a dependency property. does it have a binding?
                    //If yes, is it bound to the property we're interested in?
                    Binding bb = BindingOperations.GetBinding(parent, dp);
                    if (bb != null)
                    {
                        // This element has a DependencyProperty that we know of that is bound to the property we're interested in. 
                        // Now we just tell the callback and the caller will handle it.
                        if (parent is FrameworkElement)
                        {
                            //TODO: !!!!
                            //if (((FrameworkElement)parent).DataContext == this.DataContext)
                            //if(bb.ElementName !=null)
                            {
                                callbackDelegate((FrameworkElement)parent, bb, dp);
                            }
                        }
                    }
                }
            }

            // Now, recurse through any child elements
            if (parent is FrameworkElement || parent is FrameworkContentElement)
            {
                foreach (object childElement in LogicalTreeHelper.GetChildren(parent))
                {
                    if (childElement is DependencyObject)
                    {
                        FindBindingsRecursively((DependencyObject)childElement, callbackDelegate);
                    }
                }
            }
        }

        #endregion

        #region  Non-Static functionality 

        #region Variables

        private FrameworkElement _firstInvalidElement;
        private Dictionary<DependencyObject, Style> _backupStyles = new Dictionary<DependencyObject, Style>();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ErrorProvider()
        {            
            this.DataContextChanged += new DependencyPropertyChangedEventHandler(ErrorProvider_DataContextChanged);
            this.Loaded += new RoutedEventHandler(ErrorProvider_Loaded);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates all properties on the current data source.
        /// </summary>
        /// <returns>True if there are no errors displayed, otherwise false.</returns>
        /// <remarks>
        /// Note that only errors on properties that are displayed are included. Other errors, such as errors for properties that are not displayed, 
        /// will not be validated by this method.
        /// </remarks>
        public bool Validate1()
        {
            bool isValid = true;
            _firstInvalidElement = null;

            if (this.DataContext is IDataErrorInfo)
            {
                List<Binding> allKnownBindings = ClearInternal(this.Parent);

                // Now show all errors
                foreach (Binding knownBinding in allKnownBindings)
                {
                    string errorMessage = ((IDataErrorInfo)this.DataContext)[knownBinding.Path.Path];
                    if (errorMessage != null && errorMessage.Length > 0)
                    {
                        isValid = false;

                        // Display the error on any elements bound to the property
                        FindBindingsRecursively(this.Parent,
                        delegate(FrameworkElement element, Binding binding, DependencyProperty dp)
                        {
                            if (knownBinding.Path.Path == binding.Path.Path)
                            {

                                BindingExpression expression = element.GetBindingExpression(dp);
                                ValidationError error = new ValidationError(new ExceptionValidationRule(), expression, errorMessage, null);
                                System.Windows.Controls.Validation.MarkInvalid(expression, error);

                                if (_firstInvalidElement == null)
                                {
                                    _firstInvalidElement = element;
                                }
                                return;

                            }
                        });
                    }
                }
            }
            return isValid;
        }

        /// <summary>
        /// Returns the first element that this error provider has labelled as invalid. This method 
        /// is useful to set the users focus on the first visible error field on a page.
        /// </summary>
        /// <returns></returns>
        public FrameworkElement GetFirstInvalidElement()
        {
            return _firstInvalidElement;
        }

        /// <summary>
        /// Clears any error messages.
        /// </summary>
        public void Clear()
        {
            ClearInternal(this.Parent);
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Called when this component is loaded. We have a call to Validate here that way errors appear from the very 
        /// moment the page or form is visible.
        /// </summary>
        private void ErrorProvider_Loaded(object sender, RoutedEventArgs e)
        {
            Validate(this.Parent);
        }

        /// <summary>
        /// Called when our DataContext changes.
        /// </summary>
        private void ErrorProvider_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null && e.OldValue is INotifyPropertyChanged)
            {
                ((INotifyPropertyChanged)e.NewValue).PropertyChanged -= new PropertyChangedEventHandler(DataContext_PropertyChanged);
            }

            if (e.NewValue != null && e.NewValue is INotifyPropertyChanged)
            {
                ((INotifyPropertyChanged)e.NewValue).PropertyChanged += new PropertyChangedEventHandler(DataContext_PropertyChanged);
            }

            Validate(this.Parent);
        }


        /// <summary>
        /// Called when the PropertyChanged event is raised from the object we are bound to - that is, our data context.
        /// </summary>
        private void DataContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsValid")
            {
                return;
            }
            Validate(this.Parent);
        }

        #endregion

        #endregion
    }

    #endregion
}
