using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private LayerMask interactablesLayer;
    [SerializeField] private LayerMask grassLayer;
    [SerializeField] private LayerMask playerLayer;

    public LayerMask CollisionLayer() { return collisionLayer; }
    public LayerMask InteractableLayer() { return interactablesLayer; }
    public LayerMask GrassLayer() { return grassLayer; }
    public LayerMask PlayerLayer() { return playerLayer; }

    public static GameLayers gameLayersInstance;

    private void Awake()
    {
        gameLayersInstance = this;
    }
}
