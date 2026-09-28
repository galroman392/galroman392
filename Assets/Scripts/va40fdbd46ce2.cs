using UnityEngine;






public sealed class va40fdbd46ce2 : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private bool _visibleOnStart;

    private void Start()
    {
        this.v7d801e945b7e(this._visibleOnStart);
    }

    public void v7d801e945b7e(bool v4adc1c209583)
    {
        if (this._content != null)
        {
            this._content.SetActive(v4adc1c209583);
        }
    }
}
