/// <summary>
/// Just a container to hold stuff for a UI bar that depletes and gets added to.
/// </summary>
[System.Serializable]
public class BarContainer
{
    public float maxBarSize, minBarSize, currentBarSize, amountToAdd, amountToDeplete;

    //public BarContainer(float maximumBarSize, float initalBarSize, float amountToAddEveryUpdate, float amountToDepleteEveryUpdate)
    //{
    //    maxBarSize = maximumBarSize;
    //    currentBarSize = initalBarSize;
    //    amountToAdd = amountToAddEveryUpdate;
    //    amountToDeplete = amountToDepleteEveryUpdate;
    //}

    /// <summary>
    /// Call this in your update method to update the bar with it's deplete/adding
    /// </summary>
    public void UpdateBar()
    {
        currentBarSize += amountToAdd;
        currentBarSize -= amountToDeplete;
        ConstrainCurrentBar();
    }

    /// <summary>
    /// Constrains the currentBarSize to the maximum and minimum (0)
    /// </summary>
    private void ConstrainCurrentBar()
    {
        if (currentBarSize > maxBarSize)
            currentBarSize = maxBarSize;
        if (currentBarSize < minBarSize)
            currentBarSize = minBarSize;
    }

    /// <summary>
    /// To add some value to the current bar. Can pass negative values to subtract from it.
    /// Will constrain the value to the maximum/0
    /// </summary>
    /// <param name="toAdd">what you want to add/subtract to the bar</param>
    public void AddToCurrent(float toAdd)
    {
        currentBarSize += toAdd;
        ConstrainCurrentBar();
    }
     
    /// <summary>
    /// Will only add/subtract if it does not go past the minimum/maximum
    /// </summary>
    /// <param name="toAdd">what you want to add/subtract to the bar</param>
    public bool AddToCurrentIfLegal(float toAdd)
    {
        if (currentBarSize + toAdd <= maxBarSize && currentBarSize + toAdd >= minBarSize)
        {
            currentBarSize += toAdd;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Gets the fill amount for quick use in Unity UI images.
    /// </summary>
    /// <returns>a float between 0 and 1</returns>
    public float GetFillPercentage()
    {
        return (currentBarSize - minBarSize) / (maxBarSize - minBarSize);
    }
}
