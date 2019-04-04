using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SetupView
{

    public InputField nameInputField;
    public Text nameMessageText;

    public InputField jobInputField;
    public Text jobMessageText;

    public InputField atkInputField;
    public Text atkMessageText;

    public Slider successRateSlider;
    public Text successRateMessageText;

    public Toggle joinToggle;
    public Button joinInButton;
    public Button waitButton;

}

public class BindableProperty<T>
{
    public delegate void ValueChangedHandler(T oldValue, T newValue);

    public ValueChangedHandler OnValueChanged;

    private T _value;
    public T Value
    {
        get
        {
            return _value;
        }
        set
        {
            if (!object.Equals(_value, value))
            {
                T old = _value;
                _value = value;
                ValueChanged(old, _value);
            }
        }
    }

    private void ValueChanged(T oldValue, T newValue)
    {
        if (OnValueChanged != null)
        {
            OnValueChanged(oldValue, newValue);
        }
    }

    public override string ToString()
    {
        return (Value != null ? Value.ToString() : "null");
    }
}

//public class SetupViewModel : ViewModel
//{
//    public BindableProperty<string> Name = new BindableProperty<string>();
//    public BindableProperty<string> Job = new BindableProperty<string>();
//    public BindableProperty<int> ATK = new BindableProperty<int>();
//    public BindableProperty<float> SuccessRate = new BindableProperty<float>();
//    public BindableProperty<State> State = new BindableProperty<State>();
//}
