# Convention Handbook

1. Commit Message format
    - Commit titles start with a capital letter and end with a fullstop.
    - Commit titles should be strictly one sentence long.
    - Additional content needed should be placed in the commit description.

    **Example(s):**
    ```
    This is a commit title.
    ```
    
    ```
    This is a commit title.

    This is the additional description needed.
    ```
---
2. Commit behavior
    - All commits need to work without further modifications.
    - When someone checkouts a commit (using a detached HEAD), the project should still compile and run mostly free of any errors (semantic or otherwise).
    - Check the build before making any commits and pushes.
---
3. Code horizontal formatting and wrapping behavior
    - A line should be strict capped at a length of 80 (range: [0, 80]).
    - Manually wrapping is to be done should a line need more than 80 characters, as such:
        - Operator to be left at the previous line.
        - Where `L` is the original line, `L + 1` should be have a `+1` indent relative to `L`.
        - When more than 1 round of wrapping is to be done, `L + 2` and beyond should keep the same indentaion level as `L + 1`.

        **Example(s):**
        ```csharp
        SuperDuperLongFunctionCall(veryVeryVeryVerboseArg1,
            veryVeryVeryVerboseArg2, veryVeryVeryVerboseArg3,
            veryVeryVeryVerboseArg4);
        ```

---
4. Access modifiers
    - Explicit access modifier for all declarations, except within interfaces.
---
5. Comments format
    - All comments to start with `//` (per line needed).
    - It should than be followed by strictly 1 space character.
    - The first character after the space should be captialized should it be [a-z].
    - Comment line horizontal length should be around the same as the target code block.
    - If the comment references an identifier in the code surround the identifier in backticks.

    **Example(s):**
    ```csharp
    // BAD!
    // This is a very long and detailed comment as to why I have a bool here
    private bool _extraBool = false;

    // CORRECT
    // This is a very long and detailed
    // comment as to why I have a bool here
    private bool _extraBool = false;
    ```
    ```csharp
    private void Foo()
    {
        ...
        // BAD!
        // Call Bar to init ...

        // CORRECT
        // Call `Bar()` to init...
        Bar();
        ...
    }
    ```
---
6. Identifiers (top rules takes priority over bottom ones)
    - Source files and Assets: `PascalCase`.
    - Types: `PascalCase`.
    - Private variables: `_camelCase` (note the leading underscore).
    - Functions (including properties): `PascalCase`.
    - All other identifiers: `camelCase`.
---
7. Regions
    - Use of regions such as `Serialized Fields`, `Private Fields`, `Private Functions` and any others where appropriate.
    - All region directives should have 1 empty line above and below it, except when the top or bottom is an opening or closing brace.

    **Example(s):**
    ```csharp
    public class PlayerController : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField]
        private float _movementSpeed = 5f;

        #endregion

        #region Private Fields

        private Rigidbody _rigidbody;

        #endregion

        #region Private Functions

        private void Start()
        {
            ...
        }

        private void FixedUpdate()
        {
            ...
        }

        #endregion
    }
    ```
---
8. Create Asset Menu attribute
    - Fill in both menu name and file name.

    **Example(s):**
    ```csharp
    [CreateAssetMenu(menuName = "Scriptable Object/Entity/Creature",
        fileName = "New Creature")]
    public class Creature : Entity
    {
        ...
    }
    ```
---
9. Braces format
    - Braces should always be on its own new line.

    **Example(s):**
    ```csharp
    if (true)
    {
        Debug.Log("Correct");
    }
    ```
---
10. Attribute format
    - All attributes to have their own line.
    - Rule 3 still applies for attributes.
    - Header attribute should have 1 empty line above and below it, except when the top or bottom is an opening or closing brace.

    **Example(s):** (Rule 7 and 14 omitted to enhance clarity)
    ```csharp
    private bool _someOtherJunk = true;

    [Header("Vitals Drain Rate")]

    [SerializeField]
    [Range(0f, 5f)]
    private float _hungerDrainPerSec = 1f;
    ```
---
11. Brace omission
    - No brace omission is allowed even in cases like a 1 liner if branch.

    **Example(s):**
    ```csharp
    // BAD!
    if (true)
        return ApplyDamage(e);

    // CORRECT
    if (true)
    {
        return ApplyDamage(e);
    }
    ```
---
12. Superfluous `using` and unused Unity Message methods to be removed
---
13. Method declaration order
    - Methods to be declared in the following order:
        - Public.
        - Protected.
        - Private.
        - Should other access modifiers be used, the order can be decided then.
---
14. Variable declaration order
    - Variables to be declared in the following order:
        - Public (should have 1 empty line above and below it, except when the top or bottom is an opening or closing brace).
        - Private and Serialized (whole block should have 1 empty line above and below it, except when the top or bottom is an opening or closing brace).
        - Private and for internal use only (group all together with the whole group having 1 empty line above and below it, except when the top or bottom is an opening or closing brace).

    **Example(s):** (Rule 7 omitted to enhance clarity)
    ```csharp
    public class PlayerVitals : MonoBehaviour
    {
        public float stinkyFloat = 0f;

        [SerializeField]
        [Range(0f, 5f)]
        private float _hungerDrainPerSec = 1f;

        [SerializeField]
        [Range(0f, 5f)]
        private float _healthDrainPerSec = 0f;

        // Amount of health drain per second
        // when hunger is 0
        [SerializeField]
        [Range(0f, 10f)]
        private float _healthHungerZero = 5f;

        [SerializeField]
        private UICanvas.ActiveIn _updateActiveIn;

        private GameReset _gameReset;
        private UIManager _manager;

        private void SomeFunction()
        {
        ...
        }
    }
    ```
---
15. Commit size and frequency
    - Commits should be small in size and self-contained.
    - Commits should as best as possible not do more than 1 "big" thing as once.
    - You should commit and push frequently.
---
16. Use asserts where appropriate to document assumed predicate(s)
    - An assert message should be present in most of the cases

    **Example(s):**
    ```csharp
    private void LoadKeyframes(List<int> indices)
    {
        Assert.IsTrue(indices.Count > 0,
            "`indices` should not be empty as total " +
            "keyframes must be greater than 0");

        ...
    }
    ```
---
17. Comment label format
    - Use of labels such as `TODO`, `FIXME`, `SAFETY` and any others where appropriate.
    - Format them as such `// NAME_OF_LABEL (NAME_OF_PERSON): Comment description`.

    **Example(s):**
    ```
    // TODO (Cheng Jun): Add descriptive comments here outlining how this works
    // FIXME (Chris): A valid input is assumed here, add a validation check
    // SAFETY (Brandon): The index is presumed to be in range, due to ...
    ```
---
18. Branch usage
    - Do all development on the respective topic or feature branch, else commit to the `dev` branch.
    - `main` branch should only contain "release ready` builds that have been sufficiently tested.
---
19. Branch name
    - Branch names should only contain lowercase letters (numbers may be permitted where appropriate) and be delimited by a single dash (`-`).

    **Examples(s):**
    ```
    this-is-a-valid-branch-name
    This-Is_not-a_valid_branch-name
    ```
---
20. Generic argument identifiers
    - Prefer the use of semantic generic argument identifiers

    **Example(s):**
    ```csharp
    // BAD!
    public class StateMachine<T, U>
    {
        ...
    }
    
    // CORRECT
    public class StateMachine<TSelfID, TStateID>
    {
        ...
    }
    ```
