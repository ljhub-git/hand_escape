using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    // ���� ������Ʈ�� ���� �����ϴ� ������Ʈ�� ������ ���� ����ü.
    [System.Serializable]
    public struct PuzzleMapping
    {
        public PuzzleObject puzzleObject;
        public List<PuzzleReactObject> reactiveObjects;
    }


    /// <summary>
    /// ���� ������Ʈ�� ���� �����ϴ� ������Ʈ���� ���. �ν����� �󿡼� ������ �� ����.
    /// </summary>
    [SerializeField]
    private List<PuzzleMapping> puzzleMapping = null;

    /// <summary>
    /// Key ���� ������ Ǯ���� List�� �������� IReactToPuzzle�� ������ ������Ʈ���� ��ȭ�Ѵ�.
    /// </summary>
    private Dictionary<PuzzleObject, List<PuzzleReactObject>> puzzleMap = null;

    #region Public Func

    /// <summary>
    /// ������ Ǯ���� �� �ش� ������Ʈ�� ����� ���� ������Ʈ�鿡�� ������ ȣ���ϴ� �Լ�.
    /// </summary>
    /// <param name="_puzzleObj">������ Ǯ�� ������Ʈ.</param>
    public void OnSolvePuzzle(PuzzleObject _puzzleObj)
    {
        List<PuzzleReactObject> reactComps = null;
        puzzleMap.TryGetValue(_puzzleObj, out reactComps);

        foreach (var reactToSolve in reactComps)
        {
            reactToSolve.OnPuzzleSolved();
        }
    }

    #endregion

    #region Private Func

    /// <summary>
    /// puzzleMapping Ŭ���� ����Ʈ�� ������ ���� puzzmeMap ��ųʸ��� �ʱ�ȭ�ϴ� �Լ�.
    /// </summary>
    /// <param name="_mappings">puzzleMapping Ŭ���� ����Ʈ</param>
    private void InitializePuzzleMap(List<PuzzleMapping> _mappings)
    {
        foreach (PuzzleMapping mapping in _mappings)
        {
            PuzzleObject puzzleObj = mapping.puzzleObject;

            // PuzzleMapping Ŭ������ ���� ������Ʈ�� IPuzzleObject�� �������� ��� ���.
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
    }

    private void Start()
    {
        InitializePuzzleMap(puzzleMapping);
    }

    #endregion

}
