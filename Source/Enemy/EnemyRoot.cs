using Nyxpiri.ULTRAKILL.NyxLib;
using UnityEngine;

/* component to be added to the root/top-level game object for all enemies */
public class EnemyRoot : MonoBehaviour
{
    public EnemyComponents Enemy { get; internal set; } = null;

    protected void Awake()
    {
    }
}