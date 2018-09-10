using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Text.RegularExpressions;

namespace LinqShow
{
    class Program
    {
        static IEnumerable<string> StrCollection0 = new List<string> { "a", "bc", "d", "abc", "a", "defgh", "pqr", "xyz" };
        static IEnumerable<string> StrCollection1 = new List<string> { "1", "2", "3" };
        static IEnumerable<string> StrCollection2 = new List<string>();
        static IEnumerable<string> StrCollection3 = new List<string> { "3", "4", "5", "4" };
        static IEnumerable<string> StrCollection4 = new List<string> { "aa", "b", "cd" };
        static IEnumerable<string> StrCollection5 = new List<string> { "xy", "z", "pqr", "xy" };
        static IEnumerable<IEnumerable<string>> Str2DCollection = new List<List<string>>
        {
            new List<string> { "a", "ab", "ac" },
            new List<string> { "b", "bb" },
            new List<string> { "c", "ch", "cm" },
            new List<string> { "d", "da" }
        };
        static IEnumerable<int> IntCollection0 = new List<int> { 4, 21, 5, 0, 9, 12, 18, 7 };
        static IEnumerable<double> DoubleCollection0 = new List<double> { 2.4, 5.3, 9.1, 0.5, 6.4 };
        static IEnumerable<Alias> AliasCollection0 = new List<Alias>
        {
            new Alias { Name = "Wang", NickName = "PlayBoy" },
            new Alias { Name = "Zhang", NickName = "CleverDude" },
            new Alias { Name = "Li", NickName = "LazyOne" },
            new Alias { Name = "Wang", NickName = "GoodLooking" },
            new Alias { Name = "Chen", NickName = "FootballFan" },
            new Alias { Name = "Li", NickName = "WakeUpLate" }
        };
        static IEnumerable<Alias> AliasCollection1 = new List<Alias>
        {
            new Alias { Name = "Zhang", NickName = "Star" },
            new Alias { Name = "Wang", NickName = "Runner" },
            new Alias { Name = "Zhao", NickName = "Strong" } };
        static IEnumerable<BaseClass> DerivedCollection = new List<BaseClass>
        {
            new BaseClass(0),
            new BaseClass(1),
            new DerivedClass(2),
            new DerivedClass(3)
        };
        static UserDefineList<string> UserDefineCollection = new UserDefineList<string> { "a", "b", "c" };
        
        static void Main(string[] args)
        {
            MappingAndFiltering();
            Aggregate();
            AllAny();
            Numeric();
            Cast();
            BiCollectionsOperation();
            Contains();
            Count();
            DefaultIfEmpty();
            Distinct();
            Position();
            Initialize();
            Set();
            Group();
            Join();
            OrderBy();
            SkipTake();
            To();
            Others();
        }
                       
        static void MappingAndFiltering()
        {
            // Mapping each element in StrCollection0 into its length.
            // {1, 2, 1, 3, 1, 5, 3, 3}
            IEnumerable<int> stringLengthListVariant = Enumerable.Select(StrCollection0, x => x.Length);
            IEnumerable<int> stringLengthList = StrCollection0.Select(x => x.Length);

            // Mapping each string in StrCollection0 into another string.
            // The format is {index}+" "+{length of the string}
            // x here is the string, y here is the index.
            // {"0 1", "1 2", "2 1", "3 3", "4 1", "5 5", "6 3", "7 3"}
            IEnumerable<string> stringLengthListWithIndex = StrCollection0.Select((x, y) => y + " " + x.Length);

            // Filtering the strings in StrCollection0 and keep the ones with length longer than 2.
            // {"abc", "defgh", "pqr", "xyz"}
            IEnumerable<string> stringWithLengthLongerThan2 = StrCollection0.Where(x => x.Length > 2);
            // Filtering the strings in StrCollection0 and keep the ones with length longer than 2 and index is even.
            // {"pqr"}(length 3, index 6)
            IEnumerable<string> stringWithLengthLongerThan2WithIndex = StrCollection0.Where((x, y) => x.Length > 2 && y % 2 == 0);

            // Select the chars from the strings in the StrCollection0.
            // { 'a', 'b', 'c', 'd', 'a', 'b', 'c', 'a', 'd', 'e', ...}
            IEnumerable<char> charsFromStringList = StrCollection0.SelectMany(x => x);
            // Each of the integers from the integer list maps to an IEnumerable<int>. Then flattern them.
            // 1. {4, 21, 5, 0, 9, ...}
            // 2. {{0, 1, 2, 3}, {0, 1, 2, ... 20}, {0, 1, 2, 3, 4}, {0}, {0, 1, 2, ..., 8}, ...}
            // 3. { 0, 1, 2, 3, 0, 1, 2, ... , 20, 0, 1, 2, 3, 4, 0, 0, 1, 2,... , 8, ...}
            IEnumerable<int> integersFromIntegerList = IntCollection0.SelectMany(x => Enumerable.Range(0, x));
            // Similar to the Select with index.
            // Now the target of SelectMany is {"a0", "bc1", "d2", ...}
            // {'a', '0', 'b', 'c', '1', 'd', '2', ...}
            IEnumerable<char> charsAndIndicesFromStringList = StrCollection0.SelectMany((x, y) => x + y);

            // The first argument x=>x means mapping all the strings in StrCollection0 to itself.
            // The second argument (x, y)=>x+" "+y , x here is the string("a", "bc", "d", ...), and y here is the char('a', 'b', 'c', 'd', ...)
            // 1. {"a", "bc", "d", "abc", ...}
            // 2. {{"a"+" "+'a'}, {"bc"+" "+'b', "bc"+" "+'c'}, {"d"+" "+'d'}, {"abc"+" "+'a', "abc"+" "+'b', "abc"+" "+'c'}...}
            // 3. {"a a", "bc b", "bc c", "d d", "abc a", "abc b", "abc c"...}
            IEnumerable<string> stringAndCharsFromStringList = StrCollection0.SelectMany(x => x, (x, y) => x + " " + y);
            // There is another overload is with the index.
        }

        static void Aggregate()
        {
            // The longest string in StrCollection0.
            // "defgh"
            string maxLengthStr = StrCollection0.Aggregate((x, y) => x.Length > y.Length ? x : y);
            // The longest string length, while length at least 0.
            // 5
            int longestLengthAtLeast10 = StrCollection0.Aggregate(0, (x, y) => x >= y.Length ? x : y.Length);
            // Same as above, but the result is output in string format.
            string longestLengthAtLeast10Str = StrCollection0.Aggregate(10, (x, y) => x >= y.Length ? x : y.Length, x => x.ToString());
        }

        static void AllAny()
        {
            // Testify if ALL of the strings in StrCollection0 is shorter than 10.
            // true
            bool allStringsAreShorterThan10 = StrCollection0.All(x => x.Length < 10);
            // Testify if ALL of the ints in IntCollection0 has a length greater than 5.
            // false
            bool allIntsAreGreaterThan5 = IntCollection0.All(x => x > 5);

            // Testify if the StrCollection0 contains any elements.
            // true
            bool anyStringAreLongerThan10 = StrCollection0.Any();
            // Testify if ANY of the ints in IntCollection0 is smaller than 5.
            // True
            bool anyIntsAreSmallerThan5 = IntCollection0.Any(x => x < 5);
        }

        static void Numeric()
        {
            // The average value of IntCollection0.
            // 9.5
            double avgOfInts = IntCollection0.Average();
            // The average value of DoubleCollection0.
            // 4.74
            double avgOfDoubles = DoubleCollection0.Average();
            // The average length of the StrCollection0.
            // 2.375
            double avgLengthOfStrings = StrCollection0.Average(x => x.Length);
            // There are some other overloads for different number types(float, decimal...)

            // The Max, Min, Sum have similar usages.
            int maxOfInts = IntCollection0.Max();
            double maxOfDoubles = DoubleCollection0.Max();
            int maxLengthOfStrings = StrCollection0.Max(x => x.Length);

            int minOfInts = IntCollection0.Min();
            double minOfDoubles = DoubleCollection0.Min();
            int minLengthOfStrings = StrCollection0.Min(x => x.Length);

            int sumOfInts = IntCollection0.Sum();
            double sumOfDoubles = DoubleCollection0.Sum();
            int sumLengthOfStrings = StrCollection0.Sum(x => x.Length);
        }

        static void Cast()
        {
            Regex NumReg = new Regex("[0-9]+");
            string testString = "123abc45def6ghi7890jk";
            // matches: MatchCollections.
            // matches is NOT IEnumerable, although it is possible to call by index: matches[0], matches[1], ...etc
            // But we cannot use linq functions on matches, e.g. matches.All() or matches.Any()
            var matches = NumReg.Matches(testString);
            // matchList is IEnumerable.
            var matchList = matches.Cast<Match>();
            // Now we can use linq.
            // If there are any of the sub strings which consist of number only, and has a length longer than 3.
            // ture(7890)
            bool anyNumberSubStringIsLongerThan3 = matchList.Any(x => x.Value.Length > 3);            
        }

        static void BiCollectionsOperation()
        {
            // Concatenate two IEnumerables of the same type. The order is preserved.
            // {"a", "bc", ... "xyz", "1", "2", "3"}
            var newList = StrCollection0.Concat(StrCollection1);

            // Compare if each of the elements in array and IEnumerable are the same.
            // True.
            string[] strArray = StrCollection0.ToArray();
            bool equal = strArray.SequenceEqual(StrCollection0);
            // There is another overload under using the self-define equalilty.

            // Merge StrCollection1 and StrCollection4
            // {"1 aa", "2 b", "3 cd"}
            IEnumerable<string> zipStrings = StrCollection1.Zip(StrCollection4, (x, y) => x + " " + y);
        }

        static void Contains()
        {
            // If the StrCollection0 contains the string "abc"
            // true
            bool listContainsThisString = StrCollection0.Contains("abc");

            Alias newName = new Alias { Name = "Zhang", NickName = "Star" };
            // By default, the AliasList doesn't contain an element with Name "Zhang" and NickName "Star".
            // false
            bool aliasListContains = AliasCollection0.Contains(newName);
            // If the AliasList contains the person with Name "Zhang" and NickName "Star", under the definition of SamePerson
            // SamePerson defines the two Aliases are considered the same iff they have the same Name.
            // true.
            bool aliasListContainsSamePerson = AliasCollection0.Contains(newName, new SamePerson());
        }

        static void Count()
        {
            // The count of the StrCollection0
            // 8
            int StrCollectionCount = StrCollection0.Count();
            // The count of the string "a" in StrCollection0
            // 2(Since two of the strings are "a")
            int StrCollectionCountWithCondition = StrCollection0.Count(x => x == "a");

            // LongCount is for long list.
            long StrCollectionCountLong = StrCollection0.LongCount();
            long StrCollectionCountLongWithCondition = StrCollection0.LongCount(x => x == "a");
        }

        static void DefaultIfEmpty()
        {
            // Return the collection with default value.
            // {null}
            IEnumerable<string> emptyDefault0 = Enumerable.Empty<string>().DefaultIfEmpty();
            // Return the collection with default value, which is "NA".
            // {"NA"}
            IEnumerable<string> emptyDefault1 = Enumerable.Empty<string>().DefaultIfEmpty("NA");

            // For non empty list, return the same list.
            // { "a", "bc", "d", "abc", "a", "defgh", "pqr", "xyz" }
            IEnumerable<string> nonEmptyDefault0 = StrCollection0.DefaultIfEmpty();
            IEnumerable<string> nonEmptyDefault1 = StrCollection0.DefaultIfEmpty("NA");
        }

        static void Distinct()
        {
            // Get the distincted string list.
            // From { "a", "bc", "d", "abc", "a", "defgh", "pqr", "xyz" }
            // To { "a", "bc", "d", "abc",    "defgh", "pqr", "xyz" }
            // The second "a" is removed.
            IEnumerable<string> distinctStrCollection = StrCollection0.Distinct();

            // By default, all the aliases in AliasList is different, so nothing removed.
            IEnumerable<Alias> distinctAliasList = AliasCollection0.Distinct();
            // Under the SamePerson definition, only the following(first occurance) are kept:
            // { Name="Wang", NickName="PlayBoy"},
            // { Name = "Zhang", NickName = "CleverDude" },
            // { Name = "Li", NickName = "LazyOne" },
            // { Name = "Chen", NickName = "FootballFan" },
            IEnumerable<Alias> distinctAliasListSamePerson = AliasCollection0.Distinct(new SamePerson());
        }

        static void Position()
        {
            // Return the 1st(zero based) element of the StrCollection1
            // "2"
            string elementAtOne = StrCollection1.ElementAt(1);
            // Return the 10th element of the StrCollection1
            // Throw ArgumentOutOfRangeException
            // string elementAtTen = StrCollection1.ElementAt(10);
            // Return the 10th element of the StrCollection1, if index is invalid, return the default value.
            // null
            string elementAtTenDefault = StrCollection1.ElementAtOrDefault(10);

            // Return the first element of StrCollection0
            // "a"
            string firstElement = StrCollection0.First();
            // Return the first element with length 3 of StrCollection0
            // "abc"
            string firstElementWithLength3 = StrCollection0.First(x => x.Length == 3);
            // Return the first element in the collection. If collection is empty, return default value
            // null
            string firstElementOrDefault = Enumerable.Empty<string>().FirstOrDefault();
            // null
            string firstElementOrDefaultWithLength2 = Enumerable.Empty<string>().FirstOrDefault(x => x.Length == 2);

            // Return the last element of StrCollection0
            // "xyz"
            string lastElement = StrCollection0.Last();
            // Return the last element with length 1 of StrCollection0
            // "a"
            string lastElementWithLength1 = StrCollection0.Last(x => x.Length == 1);
            // Return the last element in the collection. If collection is empty, return default value
            // null
            string lastElementOrDefault = Enumerable.Empty<string>().LastOrDefault();
            // null
            string lastElementOrDefaultWithLength1 = Enumerable.Empty<string>().LastOrDefault(x => x.Length == 1);

            // Get the exactly one element in StrCollection0.
            // Throw exception.
            // string sinlgeStr = StrCollection0.Single();

            // Get the exactly one element with length 5 in StrCollection0
            // "defgh"
            string singleLength5 = StrCollection0.Single(x => x.Length == 5);

            // Get the exactly one element in StrCollection0.
            // Throw exception.
            // string singleOrDefaultStr = StrCollection0.Single();

            // Get the exactly one element with length 10 in StrCollection0
            // null
            string singleLength10OrDefault = StrCollection0.SingleOrDefault(x => x.Length == 10);
        }

        static void Initialize()
        {
            // Create an IEnumerable string without any thing inside.
            // {}
            IEnumerable<string> emptyList = Enumerable.Empty<string>();

            // Create a collection of integers, starts at 4, with 5 counts.
            // {4, 5, 6, 7, 8}
            IEnumerable<int> numberList = Enumerable.Range(4, 5);

            // Create a collection of strings, with 3 copies of "abc".
            // {"abc", "abc", "abc"}
            IEnumerable<string> repeatStringList = Enumerable.Repeat("abc", 3);
        }

        static void Set()
        {
            // The intersection between StrCollection1 and StrCollection3
            // {3}
            IEnumerable<string> intersectStrCollection = StrCollection1.Intersect(StrCollection3);
            // The intersection between AliasCollection0 and AliasCollection1, under the rule of SamePerson
            // Note: All the set operation are DISTINCT(according to the definition of set)!
            // { Name="Wang", NickName="PlayBoy"}, { Name="Zhang", NickName="CleverDude"}
            // (The first "Wang" and first "Zhang" in AliasCollection0.
            IEnumerable<Alias> intersectAliasCollection0 = AliasCollection0.Intersect(AliasCollection1, new SamePerson());
            // Note: the order reverses. 
            // { Name = "Zhang", NickName = "Star" }, { Name = "Wang", NickName = "Runner" }
            // (The first "Zhang" and "Wang" in AliasCollection1.
            IEnumerable<Alias> intersectAliasCollection1 = AliasCollection1.Intersect(AliasCollection0, new SamePerson());

            // The union between StrCollection1 and StrCollection3
            // {"1", "2", "3", "4", "5"}
            IEnumerable<string> unionStrCollection = StrCollection1.Union(StrCollection3);
            // The union between AliasCollection0 and AliasCollection1, under the rule of SamePerson
            // { Name="Wang", NickName="PlayBoy"}, { Name="Zhang", NickName="CleverDude"}, { Name="Li", NickName="LazyOne"}, { Name="Chen", NickName="FootballFan"}, { Name="Zhao", NickName = "Strong" }
            // (The first "Wang" and "Zhang" and "Li" and "Chen" in AliasCollection0 and first "Zhao" in AliasCollection1)
            IEnumerable<Alias> unionAliasList = AliasCollection0.Union(AliasCollection1, new SamePerson());

            // The relative complement between StrCollection1 and StrCollection3
            // {"1", "2"}
            IEnumerable<string> diffStrCollection = StrCollection1.Except(StrCollection3);
            // The relative complement between AliasCollection0 and AliasCollection1, under the rule of SamePerson
            // { Name="Li", NickName="LazyOne"}, { Name="Chen", NickName="FootballFan"}
            // (The first "Li" and first "Chen" in AliasCollection0)
            IEnumerable<Alias> diffAliasList = AliasCollection0.Except(AliasCollection1, new SamePerson());
        }

        static void Group()
        {
            // Group each of the strings by its length.
            // Since this group is group by string.Length, the Key is int.
            // { Key=1: "a", "d", "a" }
            // { Key=2: "bc" }
            // { Key=3: "abc", "pqr", "xyz" }
            // { Key=5: "defgh" }
            IEnumerable<IGrouping<int, string>> groupByLength = StrCollection0.GroupBy(x => x.Length);
            // Group the AliasCollection0 by itself, under the SamePerson rule.
            // { Key= {"Wang", "PlayBoy"}: {"Wang", "PlayBoy"}, {"Wang", "GoodLooking"} }
            // { Key= {"Zhang", "CleverDude"}: {"Zhang", "CleverDude"} }
            // { Key= {"Li", "LazyOne"}: {"Li", "LazyOne"}, {"Li", "WakeUpLate" } }
            // { Key= {"Chen", "FootballFan"}: {"Chen", "FootballFan"} }            
            IEnumerable<IGrouping<Alias, Alias>> groupByName = AliasCollection0.GroupBy(x => x, new SamePerson());
            // Group the StrCollection0 by its length. Then output its first char in the string.
            // { Key=1: 'a', 'd', 'a' } (The first char of "a", "d", "a")
            // { Key=2: 'b' } (The first char of "bc")
            // { Key=3: 'a', 'p', 'x' } (The first char of "abc", "pqr", "xyz")
            // { key=5: 'd' } (The first char of "defgh")
            IEnumerable<IGrouping<int,char>> groupByLengthGetChar = StrCollection0.GroupBy(x => x.Length, x => x[0]);
            // Group the StrCollection0 by its length. Then output the key and the count of each group.
            // {"1 3", "2 1", "3 3", "5 1"}
            // For Key=1, there are 3 elements, so out put "1 3".
            // etc.
            IEnumerable<string> groupByLengthGetCount = StrCollection0.GroupBy(x => x.Length, (key, group) => key + " " + group.Count());
            // Group the StrCollection0 by its length. Get the 0th char of the string. And concatenate them together as string.
            // {"1 ada", "2 b", "3 apx", "5 d"}
            // For Key=3, we got "abc", "pqr", "xyz" in this group. (x=>x.Length)
            // Then we get the 0th char in each of the strings. 'a', 'p', 'x'. (x=>x[0])
            // Then we merge them into a string: "3 apx". ((key, group)=>key+" "+ new string(group.ToArray()))
            IEnumerable<string> groupByLengthGetCountAndMergeChar = StrCollection0.GroupBy(x => x.Length, x => x[0], (key, group) => key + " " + new string(group.ToArray()));

            // There are three overloads of GroupBy are same as the three above, except for using a self-define IEqualityComparer.

            ///
            ///

            // ToLookUp acts almost the same as GroupBy, the difference is TolookUp can get the group by the Key.
            // { Key=1: "a", "d", "a" }
            // { Key=2: "bc" }
            // { Key=3: "abc", "pqr", "xyz" }
            // { Key=5: "defgh" }
            ILookup<int, string> toLookUpByLength = StrCollection0.ToLookup(x => x.Length);
            // {"a", "d", "a"}
            IEnumerable<string> groupWithLength1 = toLookUpByLength[1];
            // Similar to the GroupBy overload.
            // { Key=1: 'a', 'd', 'a' } (The first char of "a", "d", "a")
            // { Key=2: 'b' } (The first char of "bc")
            // { Key=3: 'a', 'p', 'x' } (The first char of "abc", "pqr", "xyz")
            // { key=5: 'd' } (The first char of "defgh")
            ILookup<int, char> toLookUpByLengthGetChar = StrCollection0.ToLookup(x => x.Length, x => x[0]);
            // {'a', 'p', 'x'}
            IEnumerable<char> groupOfCharWithLength3 = toLookUpByLengthGetChar[3];

            //There are two overloads of ToLookup are the same as the two above, except for using a self-define IEqualityComparer
        }

        static void Join()
        {
            // Join the StrCollection4 and StrCollection5. The strings with same length are joined.
            // 1. The collections need to be joined are StrCollection4 and StrCollection5.
            // 2. The key from the first list is each string's length.
            // 3. The key from the second list is each string's length.
            // 4. For each strings with same key, join them together.
            // 5. The way of join is "x y"
            // {"aa xy", "cd xy", "b z", "aa xy", "cd xy"}
            IEnumerable<string> joinBySameLength = StrCollection4.Join(StrCollection5, x => x.Length, x => x.Length, (x, y) => x + " " + y);
            // There is another overload of Join is using a self-define IEqualityComparer on the key.

            // Group join acts similar to Join. The difference is: the output is based on group.
            // { "aa xy", "cd xy", "b z" }
            IEnumerable<string> groupJoinBySameLengthGetFirst = StrCollection4.GroupJoin(StrCollection5, x => x.Length, x => x.Length, (x, y) => x + " " + y.First());
            // Compare to the same arguments by using join:
            // { "aa x", "cd x", "b z", "aa x", "cd x" }
            IEnumerable<string> joinBySameLengthGetFirst = StrCollection4.Join(StrCollection5, x => x.Length, x => x.Length, (x, y) => x + " " + y.First());
            // There is another overload of GroupJoin is using a self-define IEqualityComparer on the key.
        }

        static void OrderBy()
        {
            // Order the StrCollection0 with the string itself, with default small to big order.
            // { "a", "a", "abc", "bc", "d", "defgh", "pqr", "xyz" }
            var orderedStrCollection = StrCollection0.OrderBy(x => x);
            // Order the StrCollection0 with the StringOrder rule.
            // Note, here we implenment the IComparer, not IEqualityComparer.
            // { "a", "a", "d", "bc", "abc", "pqr", "xyz", "defgh" }
            var orderedStrCollectionWithStringOrder = StrCollection0.OrderBy(x => x, new StringOrder());

            // Order the StrCollection0 with the string itself, with default big to small order(desc).
            // {"xyz", "pqr", "defgh", "d", "bc", "abc", "a", "a"}
            var orderedStrCollectionDesc = StrCollection0.OrderByDescending(x => x);
            // There is another overload of OrderByDescending, by using the self-define IComparer.

            // Order the StrCollection0 first with its length, then by itself.
            //  OrderBy: {"a", "d", "a", "bc", "abc", "pqr", "xyz", "defgh"}
            //  ThenBy: {"a", "a", "d", "bc", "abc", "pqr", "xyz", "defgh"}
            var doubleOrder = StrCollection0.OrderBy(x => x.Length).ThenBy(x => x);
            // There is another overload of ThenBy, by using the self-define IComparer.

            // Similar to ThenBy, only the reverse order.
            // {"d", "a", "a", "bc", "xyz", "pqr", "abc", "defgh"}
            var doubleOrderDesc = StrCollection0.OrderBy(x => x.Length).ThenByDescending(x => x);
            // There is another overload of ThenByDescending, by using the self-define IComparer.
        }

        static void SkipTake()
        {
            // Skip the first 2 strings in StrCollection1
            // {"3"}
            IEnumerable<string> skip2List = StrCollection1.Skip(2);
            // Skip the first 10 strings in StrCollection1
            // {}
            IEnumerable<string> skip10List = StrCollection1.Skip(10);

            // Take the first 2 strings in StrCollection1
            // {"1", "2"}
            IEnumerable<string> take2List = StrCollection1.Take(2);
            // Take the first 10 strings in StrCollection1
            //{"1", "2", "3"}
            IEnumerable<string> take10List = StrCollection1.Take(10);

            // Skip all the elements if the length is less than 4.
            // {"defgh", "pqr", "xyz"} (The ones before are all less than 4, so they are skipped).
            IEnumerable<string> skipWhileShorterThan4List = StrCollection0.SkipWhile(x => x.Length < 4);

            // Skip all the elements with: length is less than 4, and index is less than 3.
            // { "abc", "a", "defgh", "pqr", "xyz" } (from "abc", the index>=3)
            IEnumerable<string> skipWhileShorterThan4AndIndexLessThan3List = StrCollection0.SkipWhile((x, y) => x.Length < 4 && y < 3);

            // Take all the elements if the length is less than 4.
            // { "a", "bc", "d", "abc", "a" }
            IEnumerable<string> takeWhileShorterThan4List = StrCollection0.TakeWhile(x => x.Length < 4);
            // There is another overload of TakeWhile with index inside, similar to the SkipWhile.
        }

        static void To()
        {
            // Change the IEnumerable to Array(with same order).
            // string Array { "a", "bc", "d", "abc", "a", "defgh", "pqr", "xyz" }
            string[] strArray = StrCollection0.ToArray();

            // Change the IEnumerable to List(with same order).
            // string List { "a", "bc", "d", "abc", "a", "defgh", "pqr", "xyz" }
            List<string> strList = StrCollection0.ToList();

            // Change the IEnumerable to Dictionary, use the integer form as key.
            // {{1, "1"}, {2, "2"}, {3, "3"}}
            Dictionary<int, string> intToStrDict = StrCollection1.ToDictionary(x => int.Parse(x));
            // There is another overload with IEqualityComparer to compare the Key.

            // Change the IEnumerable to Dictionary, use the string as key, the integer form as key.
            // {{"1", 1}, {"2", 2}, {"3", 3}}
            Dictionary<string, int> strToIntDict = StrCollection1.ToDictionary(x => x, x=>int.Parse(x));
            // There is another overload with IEqualityComparer to compare the Key.
        }

        static void Others()
        {
            // OfType filter the collection with certain type.
            // The DerivedCollection contains both BaseClass and DerivedClass, and OfType<DerivedClass>() will get all the DerivedClass elements inside.
            // { DerivedClass with Value=2, DerivedClass with Value=3}
            IEnumerable<DerivedClass> derivedInBaseList = DerivedCollection.OfType<DerivedClass>();

            // The reversed StrCollection1
            // {"3", "2", "1"}
            IEnumerable<string> reversedList = StrCollection1.Reverse();

            // Use the user defined Where.
            // "The is user define Where()." will be printed on screen.
            IEnumerable<string> userDefineWhere = UserDefineCollection.Where(x => x == "a");
            // Use the default Where.
            // Nothing will be printed.
            IEnumerable<string> defaultWhere = UserDefineCollection.AsEnumerable().Where(x => x == "a");
        }
    }

    class Alias
    {
        public string Name = string.Empty;
        public string NickName = string.Empty;
    }

    /// <summary>
    /// SamePerson implenment the IEqualityComparer.
    /// Rule:
    /// Iff two alias share the same Name, then they are same.
    /// </summary>
    class SamePerson : IEqualityComparer<Alias>
    {
        public bool Equals(Alias x, Alias y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(Alias alias)
        {
            return alias.Name.GetHashCode();
        }
    }

    /// <summary>
    /// StringOrder implenment the IComparer.
    /// Rule:
    /// The shorter string is always smaller.
    /// If two strings are with the same length, then compare each of the chars inside the string.
    /// The one with smaller ascii code is smaller.
    /// e.g.
    ///     "a" is less than "b"
    ///     "z" is less than "aa"
    ///     "ab" is less than "ac"
    ///     "abc" equals to "abc"
    /// </summary>
    class StringOrder : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x.Length < y.Length)
                return -1;
            if (x.Length > y.Length)
                return 1;
            for(int i = 0; i < x.Length; i++)
            {
                if (x[i] < y[i])
                    return -1;
                if (x[i] > y[i])
                    return 1;
            }
            return 0;
        }
    }

    class BaseClass
    {
        public int Value;
        public BaseClass(int value) { Value = value; }
    }

    class DerivedClass : BaseClass
    {
        public DerivedClass(int value) : base(value) { }
    }

    class UserDefineList<T> : List<T>
    {
        public IEnumerable<T> Where(Func<T,bool> predicate)
        {
            Console.WriteLine("The is user define Where().");
            return Enumerable.Where(this, predicate);
        }
    }
}
