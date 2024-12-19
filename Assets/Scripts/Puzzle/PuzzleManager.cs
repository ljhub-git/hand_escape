using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [System.Serializable]
    public struct PuzzleMapping
    {
        public GameObject puzzleObject;
        public List<GameObject> reactiveObjects;
    }

    [SerializeField]
    private List<PuzzleMapping> puzzleMapping = null;

    /// <summary>
    /// Key 값의 퍼즐이 풀리면 List로 보관중인 IReactToPuzzle를 구현한 오브젝트들이 변화한다.
    /// </summary>
    private Dictionary<IPuzzleObject, List<IReactToPuzzle>> puzzleMap = null;

    private void Awake()
    {
        puzzleMap = new Dictionary<IPuzzleObject, List<IReactToPuzzle>>();
    }

    private void Start()
    {
        InitializePuzzleMap(puzzleMapping);
    }

    public void OnSolvePuzzle(IPuzzleObject puzzleObject)
    {
        List<IReactToPuzzle> reactComps = puzzleMap[puzzleObject];

        foreach (var reactToSolve in reactComps)
        {
            reactToSolve.OnPuzzleSolved();
        }
    }

    /// <summary>
    /// puzzleMapping 클래스 리스트의 정보에 따라서 puzzmeMap 딕셔너리를 초기화하는 함수.
    /// </summary>
    /// <param name="_mappings">puzzleMapping 클래스 리스트</param>
    private void InitializePuzzleMap(List<PuzzleMapping> _mappings)
    {
        foreach (PuzzleMapping mapping in _mappings)
        {
            IPuzzleObject puzzleComponent = mapping.puzzleObject.GetComponent<IPuzzleObject>();

            // PuzzleMapping 클래스의 퍼즐 오브젝트가 IPuzzleObject를 구현했을 경우 경우.
            if (puzzleComponent != null)
            {
                List<IReactToPuzzle> puzzleReactComponents = new List<IReactToPuzzle>();

                foreach (GameObject obj in mapping.reactiveObjects)
                {
                    IReactToPuzzle puzzleReactcomponent = obj.GetComponent<IReactToPuzzle>();

                    if (puzzleReactcomponent != null)
                        puzzleReactComponents.Add(puzzleReactcomponent);
                }
                puzzleMap[puzzleComponent] = puzzleReactComponents;
            }
        }
    }
}
