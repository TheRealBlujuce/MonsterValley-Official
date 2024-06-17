using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BattleMon : MonoBehaviour
{

    [SerializeField] bool isPlayerPemo;
    public Pemo pemo;
    private Image pemoImage;
    [SerializeField] private BattleUI battleUI;
    [SerializeField] private Vector3 originalPos;
    [SerializeField] private Color originalCol;

    private void Awake()
    {
        pemoImage = GetComponent<Image>();
        originalCol = pemoImage.color;
    }

    public void Setup(Pemo currentPemo)
    {
        pemo = currentPemo;
        pemoImage.sprite = pemo.GetBaseMonster().GetFrontSprite();
        pemoImage.color = originalCol;
        PlayEnterAnimation();
    }

    public Pemo GetPemo()
    {
        return pemo;
    }

    public bool GetIsPlayerPemo()
    {
        return isPlayerPemo;
    }

    public BattleUI GetPemoBattleUI()
    {
        return battleUI;
    }
    
    #region Animations

        public void PlayEnterAnimation()
        {
            if (isPlayerPemo) 
            { 
                transform.localPosition = new Vector3(-100f, originalPos.y); 
            }
            else 
            { 
                transform.localPosition = new Vector3(100f, originalPos.y); 
            }

            transform.DOLocalMoveX(originalPos.x, 1f);

        }

        public void AttackAnimation()
        {
            var sequence = DOTween.Sequence();

            if (isPlayerPemo) { sequence.Append(transform.DOLocalMoveX(originalPos.x + 25f, 0.25f)); }
            else { sequence.Append(transform.DOLocalMoveX(originalPos.x - 25f, 0.25f)); }

            sequence.Append(transform.DOLocalMoveX(originalPos.x, 0.25f));
        }

        public void HitAnimation()
        {
            var sequence = DOTween.Sequence();

            if (isPlayerPemo) { sequence.Append(transform.DOLocalMoveX(originalPos.x - 10f, 0.25f)); }
            else { sequence.Append(transform.DOLocalMoveX(originalPos.x + 10f, 0.25f)); }

            sequence.Append(transform.DOLocalMoveX(originalPos.x, 0.25f));
        }

        public void FaintAnimation()
        {
            var sequence = DOTween.Sequence();

            sequence.Append(transform.DOLocalMoveY(originalPos.y - 50f, 1f));
            sequence.Join(pemoImage.DOFade(0f, 0.5f));

        }
    #endregion

}
