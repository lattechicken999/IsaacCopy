using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(AudioSource))]
public class TearsAction : MonoBehaviour
{
    private Animator _ani;
    private Rigidbody2D _rig;
    private TearsSpawner _parent;
    private Rigidbody2D _parentRigidbody;
    private AudioSource _audioSource;
    private float _range;
    private float _shotSpeed;
    private WaitForSeconds _destoryDelay;
    private float _aliveTime;
    private bool _isDestroying = false;
    private Vector2 firePosition;

    private void Awake()
    {
        _ani = GetComponent<Animator>();
        _rig= GetComponent<Rigidbody2D>();
        //눈물 터지는 애니메이션은 0.6초임
        _destoryDelay = new WaitForSeconds(0.3f);
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _audioSource.clip = SoundManager.Instance.GetSoundClip(SoundEnum.Tears);
    }
    private void OnEnable()
    {
        if (_parent == null) return;
        _range = _parent.TearRange;
        _shotSpeed = _parent.ShotSpeed;

        _aliveTime = 0;
        firePosition = transform.position;

        _ani.SetBool("Destroy",false);

        _rig.velocity = transform.TransformDirection(Vector2.up * _shotSpeed *7);
        _rig.velocity += _parentRigidbody.velocity / 3;
        _isDestroying = false ;
        _audioSource.Stop();
    }

    private void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
            _aliveTime += Time.deltaTime;
            if ((_aliveTime > 3) ||
               Vector2.Distance(firePosition, transform.position) > _range/7)
            {
                CommandDestroyTear();

            }
            if(_isDestroying)
            {
                _rig.velocity = Vector2.Lerp(_rig.velocity, Vector2.zero, 0.1f);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            CommandDestroyTear();
            collision.gameObject.GetComponent<MonsterController>().TakenDamage(_parent.Damage);
        }
    }
    private void CommandDestroyTear()
    {
        _ani.SetBool("Destroy", true);
        if(!_isDestroying)
            _audioSource.Play();
        _isDestroying = true;
        StartCoroutine(DestroyTears());
    }
    private IEnumerator DestroyTears()
    {
        yield return _destoryDelay;
        gameObject.SetActive(false);
        _parent.RechargeTears(gameObject);
        _isDestroying = false;
    }
    public void SetParent(TearsSpawner spawner)
    {
        _parent = spawner;
        _parentRigidbody = _parent.transform.parent.GetComponent<Rigidbody2D>();
    }
}
