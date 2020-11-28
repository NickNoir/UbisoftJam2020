using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineManager : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] private float c_hp;
    [SerializeField] private float _maxHp;
    [SerializeField] private Image _hpSlider;
    private Text repairTimeTxt;
    [Space]
    [SerializeField][Range(0.0001f,1f)] private float _dmgPerTime; //0.1hp dmg per t
    [SerializeField] private float _dmgTimerDelay; //0.1hp dmg per t

    [SerializeField] [Range(1f , 2f)] private float _repairDifficultyMultiplier;
    private float _repairTimeAmmount;
    private bool _needRepair = false;
    private bool _isRepairing = false;
    private bool _isWorking = true;

    Coroutine activeCoroutine = null;

    private void Awake()
    {
        Init();
    }

    void Start()
    {

    }

    public bool NeedRepair => _needRepair;

    void Init()
    {
        c_hp = _maxHp;
        _hpSlider.fillAmount = _maxHp;

        _isWorking = true;
        _needRepair = false;

        repairTimeTxt = _hpSlider.transform.GetChild(0).GetComponent<Text>();
        repairTimeTxt.text = "0";
    }

    void Update()
    {
        if(_isWorking && !_isRepairing)
        {
            StartCoroutine(UpdateHpSlider());
        }
        if(_isRepairing)
        {
            StopCoroutine(UpdateHpSlider());
            //StartCoroutine(RepairMachine());
            //Repair();
        }

        CheckMachine();
    }

    //void setActiveCouroutine(Coroutine coroutine)
    //{
    //    activeCoroutine = coroutine;

    //    StartCoroutine(activeCoroutine);
    //}


    public void StopRepairing()
    {

    }

    public void Repair()
    {
        //yield return null;

        c_hp = _maxHp;
        _hpSlider.fillAmount = c_hp;

        UpdateHPStageUIColor();

       _isRepairing = false;

        //return _isRepairing;
    }

    IEnumerator UpdateHpSlider()
    {
        while (_isWorking)
        {
            //Debug.Log(c_hp);
            c_hp = c_hp - _dmgPerTime ;

            _hpSlider.fillAmount = c_hp / 100;
            repairTimeTxt.text = _repairTimeAmmount.ToString("f1");

            UpdateHPStageUIColor();

            yield return new WaitForSecondsRealtime(_dmgTimerDelay);
        }
    }

    void UpdateHPStageUIColor()
    {

        if(c_hp >= _maxHp / 2 + 20)
        {
            _hpSlider.color = Color.green;
        }
        else if(c_hp < _maxHp / 2 + 20 && c_hp > _maxHp / 2 - 20)
        {
            _hpSlider.color = Color.yellow;
        }
        else
        {
            _hpSlider.color = Color.red;
        }
    }

    void CheckMachine()
    {

        if(c_hp <= 0)
        {
            _isWorking = false;
            _needRepair = false;
        }
        else if (c_hp <= _maxHp-5)
        {
            _isWorking = true;
            _needRepair = true;
        }

        //_repairTimeAmmount = _repairDifficultyMultiplier * (_maxHp - c_hp);

    }
}
