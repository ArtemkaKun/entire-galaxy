using System.Collections.Generic;
using YUART.Scripts.Star.Enums;

namespace YUART.Scripts.Galaxy_Manager.DataContainers
{
    /// <summary>
    /// Data class, that stores data of galaxy. 
    /// </summary>
    public sealed class GalaxyData
    {
        private int _mainStarsCount;
        private int _secondaryStarsCount;
    
        private int _starsClassOCount;
        private int _starsClassBCount;
        private int _starsClassACount;
        private int _starsClassFCount;
        private int _starsClassGCount;
        private int _starsClassKCount;
        private int _starsClassMCount;
        private int _starsClassTCount;
        private int _starsClassWCount;
        private int _starsClassRCount;
        private int _starsClassNCount;
    
        private int planetsCount;
        private int exoPlanetsCount;
        private int satellitesCount;

        private Dictionary<StarType, int> _starCountsDictionary;
        
        public GalaxyData()
        {
            _starCountsDictionary = new Dictionary<StarType, int>
            {
                {StarType.O, _starsClassOCount},
                {StarType.B, _starsClassBCount},
                {StarType.A, _starsClassACount},
                {StarType.F, _starsClassFCount},
                {StarType.G, _starsClassGCount},
                {StarType.K, _starsClassKCount},
                {StarType.M, _starsClassMCount},
                {StarType.T, _starsClassTCount},
                {StarType.W, _starsClassWCount},
                {StarType.R, _starsClassRCount},
                {StarType.N, _starsClassNCount}
            };
        }

        /// <summary>
        /// Increment count of stars by specified type.
        /// </summary>
        /// <param name="type">Type of a new star.</param>
        public void IncrementStarsCountByType(StarType type)
        {
            _starCountsDictionary[type] += 1;
        }
        
        /// <summary>
        /// Increment count of main stars.
        /// </summary>
        public void IncrementMainStarsCount()
        {
            _mainStarsCount += 1;
        }

        /// <summary>
        /// Increment count of secondary stars.
        /// </summary>
        public void IncrementSecondaryStarsCount()
        {
            _secondaryStarsCount += 1;
        }
    }
}
