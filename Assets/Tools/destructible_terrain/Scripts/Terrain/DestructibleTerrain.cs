using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;
using System;
using Vector2f = UnityEngine.Vector2;
using Vector2i = ClipperLib.IntPoint;

using int64 = System.Int64;

public class DestructibleTerrain : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private NameColor nameColor;
    public Material material;
    public Transform myTransformParent;
    public Transform myTransform;

    [Range(0.1f, 1.0f)]
    public float blockSize;

    private int64 blockSizeScaled;

    [Range(0.0f, 5.0f)]
    public float simplifyEpsilonPercent;

    [Range(1, 100)]
    public int resolutionX = 10;

    [Range(1, 100)]
    public int resolutionY = 10;

    public float depth = 1.0f;

    private float width;

    private float height;

    private DestructibleBlock[] blocks = new DestructibleBlock[0];
    private DestructibleBlock block;
    private Action actionRoll;
    private Action  actionComplete;
    private List<List<Vector2i>> solutions = new List<List<Vector2i>>();
    private List<List<Vector2i>> polygons = new List<List<Vector2i>>();
    private List<Vector2i> vertices = new List<Vector2i>();
    private static float timeCoolDownComplete = 0.25f;
    private bool isRoll = false, checkRoll = false;
    private float m_Time = 0;
    private float TimeResetTerrain = 10;
    private bool isColliderStay = false;
    private bool isEdited = false;
    System.Collections.IEnumerator IE_DelayActionAndLoop(Action action, float TimeDelay)
    {
        yield return Cache.GetWaiforSecond(TimeDelay);
        action?.Invoke();
        StartCoroutine(IE_DelayActionAndLoop(action, TimeDelay));
    }
    private void Awake()
    {
        if(myTransform == null) { myTransform = this.transform; }
    }
    public void OnInIt()
    {
        LoadColorMaterial();
        BlockSimplification.epsilon = (int64)(simplifyEpsilonPercent / 100f * blockSize * VectorEx.float2int64);

        width = blockSize * resolutionX;
        height = blockSize * resolutionY;
        blockSizeScaled = (int64)(blockSize * VectorEx.float2int64);

        Initialize();
        boxCollider.center = new Vector3(resolutionX / 2.0f, resolutionY / 2.0f, 0.25f);
        boxCollider.size = new Vector3(resolutionX, resolutionY, 1);
        isEdited = false;
    }

    public void Setresolution(int resolutionX, int resolutionY)
    {
        this.resolutionX = resolutionX;
        this.resolutionY = resolutionY;
        OnInIt();
    }
    private Character character;
    private void OnTriggerStay(Collider other)
    {
        character = null;
        character =  Cache.GetComponetCharacterInParent(other);
        if(character != null)
        {
            if(character.nameColorThis == nameColor)
            {
                isColliderStay = true;
            }
            isEdited = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        character = null;
        character = Cache.GetComponetCharacterInParent(other);
        if (character != null)
        {
            if (character.nameColorThis == nameColor)
            {
                isColliderStay = false;
            }
        }
    }
    public void LoadColorMaterial()
    {
        if (material == null)
        {
            material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        }

        switch (nameColor)
        {
            case NameColor.Pink:
                material.color = ColorSettings.Instance.color_Pink;
                break;
            case NameColor.Green:
                material.color = ColorSettings.Instance.color_Green;
                break;
            case NameColor.Blue:
                material.color = ColorSettings.Instance.color_Blue;
                break;
            case NameColor.Yeallow:
                material.color = ColorSettings.Instance.color_Yeallow;
                break;
            case NameColor.Violet:
                material.color = ColorSettings.Instance.color_Violet;
                break;
            case NameColor.Orange:
                material.color = ColorSettings.Instance.color_Orange;
                break;
        }
    }
    private int idx;
    public void Initialize()
    {
        if(blocks.Length == 0)
        {
            blocks = new DestructibleBlock[resolutionX * resolutionY];
        }

        for (int x = 0; x < resolutionX; x++)
        {
            for (int y = 0; y < resolutionY; y++)
            {
                polygons.Clear();
                vertices.Clear();
                vertices.Add(new Vector2i { x = x * blockSizeScaled, y = (y + 1) * blockSizeScaled });
                vertices.Add(new Vector2i { x = x * blockSizeScaled, y = y * blockSizeScaled });
                vertices.Add(new Vector2i { x = (x + 1) * blockSizeScaled, y = y * blockSizeScaled });
                vertices.Add(new Vector2i { x = (x + 1) * blockSizeScaled, y = (y + 1) * blockSizeScaled });

                polygons.Add(vertices);

                idx = x + resolutionX * y;

                if(blocks[idx] == null)
                {
                    block = CreateBlock();
                    blocks[idx] = block;
                }
            
                UpdateBlockBounds(x, y);

                blocks[idx].UpdateGeometryWithMoreVertices(polygons, width, height, depth);
            }
        }
    }

    public Vector2 GetPositionOffset()
    {
       
        return transform.localPosition;
    }

    private DestructibleBlock blockComp;
    private MeshCollider meshCollider;
    private GameObject childObject;
    private DestructibleBlock CreateBlock()
    {
        childObject = new GameObject();
        childObject.name = "DestructableBlock";
        childObject.layer = 11;
        childObject.transform.SetParent(transform);
        childObject.transform.localPosition = Vector3.zero;
        childObject.transform.localRotation = Quaternion.identity;
        blockComp = childObject.AddComponent<DestructibleBlock>();
        meshCollider =  childObject.AddComponent<MeshCollider>();
        meshCollider.convex = true;
        meshCollider.isTrigger = true;
        blockComp.SetMaterial(material);

        return blockComp;
    }
    int lx = 0;
    int ly = 0;
    int ux = 0;
    int uy = 0;
    private void UpdateBlockBounds(int x, int y)
    {
        lx = x;
        ly = y;
        ux = x + 1;
        uy = y + 1;

        if (lx == 0) lx = -1;
        if (ly == 0) ly = -1;
        if (ux == resolutionX) ux = resolutionX + 1;
        if (uy == resolutionY) uy = resolutionY + 1;

        BlockSimplification.currentLowerPoint = new Vector2i
        {
            x = lx * blockSizeScaled,
            y = ly * blockSizeScaled
        };

        BlockSimplification.currentUpperPoint = new Vector2i
        {
            x = ux * blockSizeScaled,
            y = uy * blockSizeScaled
        };
    }

    private int x1;
    private int y1;
    private int x2;
    private int y2;
    private bool checkAddPolygons;
    private ClipBounds bounds;
    private List<Vector2i> clipVertices;
    private ClipperLib.Clipper clipper = new ClipperLib.Clipper();
    public void ExecuteClip(IClip clip)
    {
        BlockSimplification.epsilon = (int64)(simplifyEpsilonPercent / 100f * blockSize * VectorEx.float2int64);
        isRoll = false;
        clipVertices = clip.GetVertices();
        bounds = clip.GetBounds();
        x1 = Mathf.Max(0, (int)(bounds.lowerPoint.x / blockSize));
        if (x1 > resolutionX - 1)
        {
            ActiveActionComplete();
            return;
        }
         y1 = Mathf.Max(0, (int)(bounds.lowerPoint.y / blockSize));
        if (y1 > resolutionY - 1) 
        {
            ActiveActionComplete();
            return;
        }
         x2 = Mathf.Min(resolutionX - 1, (int)(bounds.upperPoint.x / blockSize));
        if (x2 < 0) 
        {
            ActiveActionComplete();
            return;
        }
         y2 = Mathf.Min(resolutionY - 1, (int)(bounds.upperPoint.y / blockSize));
        if (y2 < 0) 
        {
            ActiveActionComplete();
            return;
        }
         checkAddPolygons = false;
        for (int x = x1; x <= x2; x++)
        {
            for (int y = y1; y <= y2; y++)
            {
                if (clip.CheckBlockOverlapping(new Vector2f((x + 0.5f) * blockSize, (y + 0.5f) * blockSize), blockSize))
                {
                    block = blocks[x + resolutionX * y];
                    solutions.Clear();
                    clipper.Clear();
                    checkAddPolygons = clipper.AddPolygons(block.Polygons, ClipperLib.PolyType.ptSubject, actionRoll, ResetActiveActionComplete);
                    clipper.AddPolygon(clipVertices, ClipperLib.PolyType.ptClip, actionRoll, ResetActiveActionComplete);
                    clipper.Execute(ClipperLib.ClipType.ctDifference, solutions,
                        ClipperLib.PolyFillType.pftNonZero, ClipperLib.PolyFillType.pftNonZero);
                    UpdateBlockBounds(x, y);
                    block.UpdateGeometryWithMoreVertices(solutions, width, height, depth);
                  
                }
               
            }
        }
        if (!checkAddPolygons)
        {
            ActiveActionComplete();
        }

    }
    public void ResetActiveActionComplete()
    {
        if (checkRoll)
        {
            m_Time = 0;
            isRoll = true;
            checkRoll = false;
        }
       
    }
    public void ActiveActionComplete() 
    {
        if (!checkRoll)
        {
            m_Time = 0;
            isRoll = false;
            checkRoll = true;
        }
      
    }
    private void Update()
    {
        if (checkRoll)
        {
            if (!isRoll)
            {
                if (m_Time <= timeCoolDownComplete)
                {                  
                    m_Time += Time.deltaTime;
                }
                else
                {
                    actionComplete?.Invoke();
                    checkRoll = false;
                    m_Time = 0;
                }
            }

        }

    }
    public void ResetAllAction()
    {
      
        actionRoll = null;
        actionComplete = null;
    }
    public void Active(bool isActive)
    {
        gameObject.SetActive(isActive);

        if (gameObject.activeInHierarchy)
        {
            ReLoadDstructibleTerrain();
        }
    }
    private void ReLoadDstructibleTerrain()
    {
        StartCoroutine(IE_DelayActionAndLoop(() =>
        {
            if (!isColliderStay && isEdited)
            {
                OnInIt();
            }
        }, TimeResetTerrain));
    }
    public void SetActionRoll(Action action)
    {
        actionRoll = action;
    }
    public void SetActionComplete(Action action)
    {
        actionComplete = action;
    }
    public NameColor GetNameColor()
    {
        return nameColor;
    }
    public void SetNameColor(NameColor nameColor)
    {
        this.nameColor = nameColor;
    }
    public enum TypeAction
    {
        Roll,
        Complete
    }
}


