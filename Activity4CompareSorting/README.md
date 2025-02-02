# Activity: Sorting Algorithm Comparison

---

# 📝 Cover Sheet  
**Student Name:** Alex Frear  
**Date:** 02/02/2025  
**Program:** College of Science, Engineering, and Technology, Grand Canyon University  
**Course:** CST-201 Algorithms and Data Structures  
**Instructor:** Mohamed Mneimneh  

---

## 🎥 **Screencast Video**
<div>
    <!-- Replace these Loom links and thumbnails with your actual video details. -->
    <a href="https://www.loom.com/share/EXAMPLE_VIDEO_LINK">
      <p>CST-201 - Sorting Algorithm Comparison - Watch Video</p>
    </a>
    <a href="https://www.loom.com/share/EXAMPLE_VIDEO_LINK">
      <img style="max-width:300px;" src="https://cdn.loom.com/sessions/thumbnails/EXAMPLE_THUMBNAIL.gif">
    </a>
</div>

---

# 📋 Sorting Algorithm Comparison

## 📄 **Description**
This console-based C# program compares **Selection Sort**, **Bubble Sort**, **Merge Sort**, **Quick Sort**, and a **Hybrid** approach (Quick Sort + Insertion Sort for small subarrays). It counts **comparisons** and **exchanges** for each algorithm over **100 runs**, then prints their **average** metrics.

---

## 📄 **Pseudocode**
### High-Level Overview
```
1. Read n from user.
2. For iteration in [1..100]:
   a. Create array of size n with values 1..n
   b. Shuffle array (Fisher-Yates)
   c. For each algorithm:
      - Clone the same shuffled array
      - Sort while counting comparisons & exchanges
      - Accumulate totals
3. Print average comparisons/exchanges for each algorithm.
```

---

## 📄 **Source Code**
- **Program.cs**  
  [View on GitHub](https://github.com/YourRepo/SortingComparison/blob/main/Program.cs)  
  *(Replace this link with your actual repository or submission URL.)*

---

## 📸 **Screenshots**

### ✅ **1. Console Prompt**
<img src="screenshots/console_prompt.png" width="700"/>

*The program waits for the user to enter the size of the array (n).*

---

### ✅ **2. Sorting Results & Explanation**
<img src="screenshots/console_results.png" width="700"/>

*After 100 iterations, the program prints each algorithm’s average comparisons and exchanges, followed by a brief explanation.*

---

# 💻 **Running the Program**
1. **Open** the project in **Visual Studio** or another C# IDE.
2. **Build/Run** (e.g., press **F5**).
3. **Enter** a positive integer for `n`.
4. **View** the average metrics for each sorting algorithm after the loop finishes.

---

## 📝 **Example Console Output**
```
Enter the size of the array (n): 100

Averages over 100 iterations:
Selection Sort: Comparisons = 4950, Exchanges = 284
Bubble Sort:    Comparisons = 4950, Exchanges = 7528
Merge Sort:     Comparisons = 541,  Exchanges = 672
Quick Sort:     Comparisons = 653,  Exchanges = 1175
Hybrid Sort:    Comparisons = 633,  Exchanges = 975

--- Brief Explanation of the Output ---
Comparisons: # of times we check if one element is less/greater than another.
Exchanges:   # of data writes (including swaps, merges, insertions).

Sorting Algorithms:
- Selection Sort (O(n^2)): Finds the minimum each pass and swaps it to the front.
- Bubble Sort (O(n^2)):    Swaps adjacent out-of-order pairs repeatedly.
- Merge Sort (O(n log n)): Recursively splits and merges; fewer compares but more writes.
- Quick Sort (O(n log n)): Partitions around a pivot; performance can vary by pivot choice.
- Hybrid Sort: A Quick Sort variant with median-of-three pivot & insertion sort on small subarrays.
```

*(Numbers may differ slightly based on random shuffling.)*

---

## 📚 **Key Points**
- **O(n^2)** sorts (Selection, Bubble) typically have more comparisons than **O(n log n)** sorts (Merge, Quick, Hybrid).  
- Each **swap** is considered **3 exchanges** (due to the 3 writes involved).  
- Merge Sort’s merging step copies elements, leading to additional single-write exchanges.  
- Hybrid Sort aims to optimize Quick Sort by switching to Insertion Sort for small partitions and using a median-of-three pivot selection.

---

## 🏆 **Potential Improvements**
- Implement additional sorts (Heap Sort, Shell Sort, etc.) for broader comparisons.  
- Graph how comparisons and exchanges scale as \(n\) grows.  
- Allow user-defined iteration counts or choice of algorithms.

---

**End of README**
