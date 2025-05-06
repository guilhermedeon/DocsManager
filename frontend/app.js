const API_BASE_URL = "http://localhost:5000";
let jwtToken = null;
let currentPage = 1;
let currentSortColumn = "";
let currentSortDescending = false;

async function fetchToken() {
    try {
        const response = await fetch(`${API_BASE_URL}/api/Auth/token`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
            },
        });
        console.log("Token response status:", response.status);
        if (!response.ok) {
            const errorText = await response.text();
            console.error("Token fetch error details:", errorText);
            throw new Error("Failed to fetch token");
        }
        const data = await response.json();
        console.log("Token response data:", data);
        jwtToken = data.token;
    } catch (error) {
        console.error("Error fetching token:", error);
        alert("Could not authenticate. Please try again later.");
    }
}

async function fetchDocuments(
    searchText = "",
    page = 1,
    sortColumn = "",
    sortDescending = false,
    pageSize = 10
) {
    try {
        const queryParams = new URLSearchParams();
        if (searchText) queryParams.append("SearchText", searchText);
        if (page) queryParams.append("PageNumber", page);
        if (sortColumn) queryParams.append("SortColumn", sortColumn);
        queryParams.append("SortDescending", sortDescending);
        queryParams.append("PageSize", pageSize);

        console.log(
            "Fetching documents with query params:",
            queryParams.toString()
        );

        const response = await fetch(
            `${API_BASE_URL}/api/Documents?${queryParams.toString()}`,
            {
                headers: {
                    Authorization: `Bearer ${jwtToken}`,
                },
            }
        );
        console.log("API response status:", response.status);

        if (!response.ok) {
            const errorText = await response.text();
            console.error("API error details:", errorText);
            throw new Error("Failed to fetch documents");
        }

        const responseData = await response.json();
        console.log("API response data:", responseData);

        const documents = responseData.documents || []; // Handle lowercase 'documents'
        const totalCount = responseData.totalCount || 0; // Handle lowercase 'totalCount'

        populateTable(documents, totalCount);
    } catch (error) {
        console.error("Error fetching documents:", error);
        alert("Could not load documents. Please try again later.");
    }
}

function populateTable(documents, totalCount) {
    console.log("Populating table with documents:", documents);
    console.log("Total count:", totalCount);

    const tableBody = document.getElementById("documentTableBody");
    if (!tableBody) {
        console.error("Table body element not found in the DOM.");
        return;
    }

    tableBody.innerHTML = "";

    if (!documents || documents.length === 0) {
        tableBody.innerHTML = `<tr><td colspan="6" class="text-center">No documents found</td></tr>`;
        return;
    }

    documents.forEach((doc) => {
        console.log("Adding document to table:", doc);
        const row = document.createElement("tr");

        row.innerHTML = `
            <td>${doc.code}</td>
            <td>${doc.title}</td>
            <td>${doc.revision}</td>
            <td>${
                doc.plannedDate
                    ? new Date(doc.plannedDate).toLocaleDateString()
                    : ""
            }</td>
            <td>${doc.value ? doc.value.toFixed(2) : ""}</td>
            <td>
                <button class="btn btn-sm btn-warning" onclick="editDocument('${
                    doc.guid
                }')">Edit</button>
                <button class="btn btn-sm btn-danger" onclick="deleteDocument('${
                    doc.guid
                }')">Delete</button>
            </td>
        `;

        tableBody.appendChild(row);
    });

    setupPagination(totalCount);
}

function setupPagination(totalCount) {
    const paginationContainer = document.getElementById("paginationContainer");
    paginationContainer.innerHTML = "";

    const pageSize = parseInt(document.getElementById("pageSize").value, 10);
    const totalPages = Math.ceil(totalCount / pageSize);

    for (let i = 1; i <= totalPages; i++) {
        const pageButton = document.createElement("button");
        pageButton.className = "btn btn-secondary mx-1";
        pageButton.textContent = i;
        pageButton.disabled = i === currentPage;
        pageButton.addEventListener("click", () => {
            currentPage = i;
            fetchDocuments(
                document.getElementById("searchInput").value.trim(),
                currentPage,
                currentSortColumn,
                currentSortDescending,
                pageSize
            );
        });
        paginationContainer.appendChild(pageButton);
    }
}

function setupTableSorting() {
    const headers = document.querySelectorAll("th.sortable");
    headers.forEach((header) => {
        header.addEventListener("click", () => {
            const column = header.dataset.column;
            if (currentSortColumn === column) {
                currentSortDescending = !currentSortDescending;
            } else {
                currentSortColumn = column;
                currentSortDescending = false;
            }
            fetchDocuments(
                document.getElementById("searchInput").value.trim(),
                currentPage,
                currentSortColumn,
                currentSortDescending,
                parseInt(document.getElementById("pageSize").value, 10)
            );
        });
    });
}

document.addEventListener("DOMContentLoaded", async () => {
    await fetchToken();

    const app = document.getElementById("app");

    app.innerHTML = `
        <div class="mb-4">
            <input type="text" id="searchInput" class="form-control" placeholder="Search by title or code" />
            <button class="btn btn-primary mt-2" id="searchButton">Search</button>
            <button class="btn btn-secondary mt-2" id="clearSearchButton">Clear</button>
        </div>
        <div class="mb-4">
            <label for="pageSize" class="form-label">Page Size:</label>
            <select id="pageSize" class="form-select">
                <option value="5">5</option>
                <option value="10" selected>10</option>
                <option value="20">20</option>
                <option value="50">50</option>
            </select>
        </div>
        <div class="mb-4">
            <button class="btn btn-primary" id="addDocument">Add Document</button>
        </div>
        <table class="table table-striped">
            <thead>
                <tr>
                    <th class="sortable" data-column="code">Code</th>
                    <th class="sortable" data-column="title">Title</th>
                    <th class="sortable" data-column="revision">Revision</th>
                    <th class="sortable" data-column="plannedDate">Planned Date</th>
                    <th class="sortable" data-column="value">Value</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody id="documentTableBody">
                <!-- Rows will be dynamically added here -->
            </tbody>
        </table>
        <div id="paginationContainer" class="mt-3"></div>
    `;

    document.getElementById("addDocument").addEventListener("click", () => {
        showDocumentForm();
    });

    document.getElementById("searchButton").addEventListener("click", () => {
        const searchText = document.getElementById("searchInput").value.trim();
        currentPage = 1;
        fetchDocuments(
            searchText,
            currentPage,
            currentSortColumn,
            currentSortDescending,
            parseInt(document.getElementById("pageSize").value, 10)
        );
    });

    document
        .getElementById("clearSearchButton")
        .addEventListener("click", () => {
            document.getElementById("searchInput").value = "";
            currentPage = 1;
            fetchDocuments(
                "",
                currentPage,
                currentSortColumn,
                currentSortDescending,
                parseInt(document.getElementById("pageSize").value, 10)
            );
        });

    document.getElementById("pageSize").addEventListener("change", () => {
        currentPage = 1;
        fetchDocuments(
            document.getElementById("searchInput").value.trim(),
            currentPage,
            currentSortColumn,
            currentSortDescending,
            parseInt(document.getElementById("pageSize").value, 10)
        );
    });

    setupTableSorting();
    fetchDocuments();
});

function validateForm(documentData) {
    if (!documentData.code || !documentData.title) {
        alert("Code and Title are required fields.");
        return false;
    }
    if (documentData.value && isNaN(documentData.value)) {
        alert("Value must be a valid number.");
        return false;
    }
    return true;
}

function showDocumentForm(doc = null) {
    const app = document.getElementById("app");
    if (!app) {
        console.error("The 'app' element is not found in the DOM.");
        alert("An error occurred. Please refresh the page.");
        return;
    }

    const formHtml = `
        <form id="documentForm">
            <input type="hidden" id="guid" name="guid" value="${
                doc?.guid || ""
            }">
            <div class="mb-3">
                <label for="code" class="form-label">Code</label>
                <input type="text" class="form-control" id="code" name="code" required value="${
                    doc?.code || ""
                }">
            </div>
            <div class="mb-3">
                <label for="title" class="form-label">Title</label>
                <input type="text" class="form-control" id="title" name="title" required value="${
                    doc?.title || ""
                }">
            </div>
            <div class="mb-3">
                <label for="revision" class="form-label">Revision</label>
                <select class="form-select" id="revision" name="revision" required>
                    <option value="0" ${
                        doc?.revision === "0" ? "selected" : ""
                    }>0</option>
                    <option value="A" ${
                        doc?.revision === "A" ? "selected" : ""
                    }>A</option>
                    <option value="B" ${
                        doc?.revision === "B" ? "selected" : ""
                    }>B</option>
                    <option value="C" ${
                        doc?.revision === "C" ? "selected" : ""
                    }>C</option>
                    <option value="D" ${
                        doc?.revision === "D" ? "selected" : ""
                    }>D</option>
                    <option value="E" ${
                        doc?.revision === "E" ? "selected" : ""
                    }>E</option>
                    <option value="F" ${
                        doc?.revision === "F" ? "selected" : ""
                    }>F</option>
                    <option value="G" ${
                        doc?.revision === "G" ? "selected" : ""
                    }>G</option>
                </select>
            </div>
            <div class="mb-3">
                <label for="plannedDate" class="form-label">Planned Date</label>
                <input type="date" class="form-control" id="plannedDate" name="plannedDate" value="${
                    doc?.plannedDate
                        ? new Date(doc.plannedDate).toISOString().split("T")[0]
                        : ""
                }">
            </div>
            <div class="mb-3">
                <label for="value" class="form-label">Value</label>
                <input type="number" class="form-control" id="value" name="value" min="0" step="0.01" value="${
                    doc?.value || ""
                }">
            </div>
            <button type="submit" class="btn btn-primary">Save</button>
            <button type="button" class="btn btn-secondary" id="cancelForm">Cancel</button>
        </form>
    `;

    app.innerHTML = formHtml;

    document.getElementById("cancelForm").addEventListener("click", () => {
        location.reload(); // Reload to reset the app view
    });

    document.getElementById("documentForm").addEventListener("submit", (e) => {
        e.preventDefault();
        const formData = new FormData(e.target);
        const documentData = Object.fromEntries(formData.entries());
        saveDocument(documentData);
    });
}

async function saveDocument(documentData) {
    if (!validateForm(documentData)) return;

    const isNew = !documentData.guid;
    const url = isNew
        ? `${API_BASE_URL}/api/Documents`
        : `${API_BASE_URL}/api/Documents/${documentData.guid}`;
    const method = isNew ? "POST" : "PUT";

    try {
        const response = await fetch(url, {
            method: method,
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${jwtToken}`,
            },
            body: JSON.stringify({
                guid: documentData.guid || undefined,
                code: documentData.code.trim(),
                title: documentData.title.trim(),
                revision: documentData.revision,
                plannedDate: documentData.plannedDate
                    ? new Date(
                          `${documentData.plannedDate}T00:00:00-03:00`
                      ).toISOString()
                    : null,
                value: documentData.value
                    ? parseFloat(documentData.value)
                    : null,
            }),
        });

        if (!response.ok) {
            const errorText = await response.text();
            console.error("Save document error details:", errorText);
            throw new Error("Failed to save document");
        }

        alert(
            isNew
                ? "Document created successfully!"
                : "Document updated successfully!"
        );
        location.reload();
    } catch (error) {
        console.error("Error saving document:", error);
        alert("Could not save document. Please try again later.");
    }
}

async function deleteDocument(guid) {
    if (!confirm("Are you sure you want to delete this document?")) return;

    try {
        const response = await fetch(`${API_BASE_URL}/api/Documents/${guid}`, {
            method: "DELETE",
            headers: {
                Authorization: `Bearer ${jwtToken}`,
            },
        });
        if (!response.ok) {
            throw new Error("Failed to delete document");
        }
        alert("Document deleted successfully!");
        fetchDocuments();
    } catch (error) {
        console.error("Error deleting document:", error);
        alert("Could not delete document. Please try again later.");
    }
}

async function editDocument(guid) {
    try {
        const response = await fetch(`${API_BASE_URL}/api/Documents/${guid}`, {
            headers: {
                Authorization: `Bearer ${jwtToken}`,
            },
        });
        if (!response.ok) {
            throw new Error("Failed to fetch document details");
        }
        const document = await response.json();
        showDocumentForm({
            guid: document.guid,
            code: document.code,
            title: document.title,
            revision: document.revision,
            plannedDate: document.plannedDate
                ? new Date(document.plannedDate).toISOString().split("T")[0]
                : "",
            value: document.value ? document.value.toString() : "",
        });
    } catch (error) {
        console.error("Error fetching document details:", error);
        alert("Could not load document details. Please try again later.");
    }
}
