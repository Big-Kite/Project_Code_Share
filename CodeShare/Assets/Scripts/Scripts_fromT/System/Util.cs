using System.Collections.Generic;
using UnityEngine;

static class YieldCache // �̸� ĳ���ص��� �ʰ� �ڵ忡�� ���ʻ�뿡 ĳ�̵Ǿ� ���Ļ�뿡 ĳ�ø� �ҷ��´�.
{
    class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y)
        {
            return x == y;
        }

        int IEqualityComparer<float>.GetHashCode(float obj)
        {
            return obj.GetHashCode();
        }
    }

    static readonly Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(new FloatComparer());

    public static WaitForSeconds WaitForSeconds(float seconds)              
    {                                                                       
        WaitForSeconds wfs;                                                 
        if (!_timeInterval.TryGetValue(seconds, out wfs))                   
            _timeInterval.Add(seconds, wfs = new WaitForSeconds(seconds));  
        return wfs;                                                         
    }                                                                       
}

static class Filper
{
    public static void Filp(Transform _body, bool _isRight)
    {
        Vector3 newFilp = _body.localScale;
        newFilp.x = Mathf.Abs(newFilp.x) * (_isRight ? -1 : 1);
        _body.localScale = newFilp;
    }
    public static void InputFilp(Transform _body, Vector3 _inputVec)
    {
        if (_inputVec.x == 0.0f) return;

        bool isRight = _inputVec.x > 0.0f;
        if (isRight && _body.localScale.x < 0.0f) return;
        if (!isRight && _body.localScale.x > 0.0f) return;

        Vector3 newFilp = _body.localScale;
        newFilp.x = Mathf.Abs(newFilp.x) * (isRight ? -1 : 1);
        _body.localScale = newFilp;
    }
    //public static void ScalingFilp(Transform _body, Vector3 _start, Vector3 _end, float _value, bool _isRight)
    //{
    //    Vector3 newFilp = Vector3.Lerp(_start, _end, _value);
    //    newFilp.x = Mathf.Abs(newFilp.x) * (_isRight ? -1 : 1);
    //    _body.localScale = newFilp;
    //}
}

// �ּ��� ����� �� ������ Ǯ��
//public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
//public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
//private static readonly Dictionary<float, WaitForSecondsRealtime> _timeIntervalReal = new Dictionary<float, WaitForSecondsRealtime>(new FloatComparer());

//public static WaitForSecondsRealtime WaitForSecondsRealTime(float seconds)
//
//WaitForSecondsRealtime wfsReal;
//if (!_timeIntervalReal.TryGetValue(seconds, out wfsReal))
//  _timeIntervalReal.Add(seconds, wfsReal = new WaitForSecondsRealtime(seconds));
//return wfsReal;
