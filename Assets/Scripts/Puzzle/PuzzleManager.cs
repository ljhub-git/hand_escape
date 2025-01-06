using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PuzzleManager : MonoBehaviour
{
    // 퍼즐 오브젝트와 퍼즐에 반응하는 오브젝트의 매핑을 위한 구조체.
    [System.Serializable]
    public struct PuzzleMapping
    {
        public PuzzleObject puzzleObject;
        public List<PuzzleReactObject> reactiveObjects;
    }

    /// <summary>
    /// 퍼즐 오브젝트와 퍼즐에 반응하는 오브젝트들의 목록. 인스펙터 상에서 편집할 수 있음.
    /// </summary>
    [SerializeField]
    private List<PuzzleMapping> puzzleMapping = null;

    /// <summary>
    /// Key 값의 퍼즐이 풀리면 List로 보관중인 IReactToPuzzle를 구현한 오브젝트들이 변화한다.
    /// </summary>
    private Dictionary<PuzzleObject, List<PuzzleReactObject>> puzzleMap = null;

    private NetworkObjectManager networkObjectManager = null;

    #region Public Func

    /// <summary>
    /// 퍼즐이 풀렸을 때 해당 오브젝트와 연결된 반응 오브젝트들에게 반응을 호출하는 함수.
    /// </summary>
    /// <param name="_puzzleObj">퍼즐이 풀린 오브젝트.</param>
    public void OnSolvePuzzle(PuzzleObject _puzzleObj)
    {
        List<PuzzleReactObject> reactComps = null;
        puzzleMap.TryGetValue(_puzzleObj, out reactComps);

        foreach (var reactToSolve in reactComps)
        {
            reactToSolve.OnPuzzleSolved();

            networkObjectManager.CallOnPuzzleSolvedToOthers(reactToSolve.GetComponent<PhotonView>());
        }
    }

    #endregion

    #region Private Func

    /// <summary>
    /// puzzleMapping 클래스 리스트의 정보에 따라서 puzzmeMap 딕셔너리를 초기화하는 함수.
    /// </summary>
    /// <param name="_mappings">puzzleMapping 클래스 리스트</param>
    private void InitializePuzzleMap(List<PuzzleMapping> _mappings)
    {
        foreach (PuzzleMapping mapping in _mappings)
        {
            PuzzleObject puzzleObj = mapping.puzzleObject;

            // PuzzleMapping 클래스의 퍼즐 오브젝트가 IPuzzleObject를 구현했을 경우 경우.
            if (puzzleObj != null)
            {
                List<PuzzleReactObject> puzzleReactObj = new List<PuzzleReactObject>();

                foreach (PuzzleReactObject obj in mapping.reactiveObjects)
                {
                    if(obj != null)
                        puzzleReactObj.Add(obj);
                }
                puzzleMap[puzzleObj] = puzzleReactObj;
            }
        }
    }

    #endregion

    #region Unity Callback Func

    private void Awake()
    {
        puzzleMap = new Dictionary<PuzzleObject, List<PuzzleReactObject>>();
        networkObjectManager = FindAnyObjectByType<NetworkObjectManager>();
    }

    private void Start()
    {
        InitializePuzzleMap(puzzleMapping);
    }

    #endregion

}
