using UnityEngine;
using UnityEngine.UI;

public class SliderRecharge : MonoBehaviour
{
    Slider _slider;
    public float rechargeDelay = 0.2f;
    float maxCharge;
    //float timer;

    void Awake()
    {
        _slider = GetComponent<Slider>();
        maxCharge = _slider.value;
    }

    void FixedUpdate()
    {
        //timer += Time.deltaTime;
        if (_slider.value >= maxCharge)
        {
            return;
        }
        /*if (_slider.value <= 10.5f && _slider.value >= 10.0f)
        {
            Debug.Log(timer);
        }
        else if (_slider.value <= 20.5f && _slider.value >= 20.0f)
        {
            Debug.Log(timer);
        }
        else if (_slider.value <= 30.5f && _slider.value >= 30.0f)
        {
            Debug.Log(timer);
        }*/
        
        _slider.value += Mathf.Lerp(0.01f, _slider.value, rechargeDelay * Time.deltaTime);
    }
}





/*ВАРИАНТ C ИСЧЕЗНОВЕНИЕМ
 * 
 * using UnityEngine;
using UnityEngine.UI;

public class SliderRecharge : MonoBehaviour
{
    Image _sliderAlpha;
    Slider _slider;
    public float alphaChange = 0.05f;
    Text _fireText;

    void Awake()
    {
        _sliderAlpha = GetComponentInChildren<Image>();
        _slider = GetComponent<Slider>();
        _fireText = GetComponentInChildren<Text>();
    }
    
    void Update()
    {
        
    }

    public void ValueChanged()
    {
        _sliderAlpha.color = new Color(_sliderAlpha.color.r, _sliderAlpha.color.g, _sliderAlpha.color.b, _slider.value * alphaChange);
        if (_sliderAlpha.color.a == 0)
        {
            _fireText.color = new Color(_fireText.color.r, _fireText.color.g, _fireText.color.b, 0);
        }
        else
        {
            _fireText.color = new Color(_fireText.color.r, _fireText.color.g, _fireText.color.b, 1);
        }
        Debug.Log("Value Changed, new alpha: " + _slider.value * alphaChange);
    }
}
*/
