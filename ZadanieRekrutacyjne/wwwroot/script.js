async function fetchTags() {
    const url = 'https://localhost:7182/api/gettags/'; 
  
    try {
      const response = await fetch(url);
      if (!response.ok) {
        throw new Error(`Error fetching tags: ${response.status}`);
      }
      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Error fetching tags:', error);
      // Handle errors gracefully, e.g., display an error message to the user
      return []; // Return an empty array in case of error (optional)
    }
  }
  
  async function getBackendTagDetails(id) {
    const url = `https://localhost:7182/api/gettags/${id}`;
  
    try {
      const response = await fetch(url);
      if (!response.ok) {
        throw new Error(`Error fetching tag details (ID: ${id}): ${response.status}`);
      }
      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Error fetching tag details:', error);
      return {}; // Return an empty object in case of error (optional)
    }
  }
  
  fetchTags()
    .then(tags => {
      const tagsList = document.getElementById('tags-list');
  
      tags.forEach(tag => {
        const id = tag.id; // Replace with appropriate ID property
  
        // Fetch additional tag details from API
        getBackendTagDetails(id)
          .then(apiTagDetails => {
            // Combine local and API tag data
            const enrichedTag = { ...tag, ...apiTagDetails };
  
            // Create list item with updated tag data
            const listItem = document.createElement('li');
            listItem.innerHTML = `<b>${enrichedTag.name}</b> (Count: ${enrichedTag.count}, Percentage: ${enrichedTag.percentage}%, Details: ${apiTagDetails.description})`;
            tagsList.appendChild(listItem);
          })
          .catch(error => {
            console.error('Error fetching tag details:', error);
            // Handle errors gracefully, e.g., display an error message for the specific tag
          });
      });
    })
    .catch(error => {
      console.error('Error fetching tags:', error);
      // Handle errors gracefully, e.g., display a general error message
    });