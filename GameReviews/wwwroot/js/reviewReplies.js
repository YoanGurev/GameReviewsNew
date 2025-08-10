document.addEventListener("DOMContentLoaded", () => {
    // Handle Show Replies button
    document.querySelectorAll(".show-replies-btn").forEach(button => {
        button.addEventListener("click", async () => {
            const reviewId = button.getAttribute("data-review-id");
            const repliesList = document.getElementById(`replies-list-${reviewId}`);

            if (repliesList.style.display === "none") {
                // Load replies via AJAX
                const response = await fetch(`/Games/GetReplies?reviewId=${reviewId}`);
                if (response.ok) {
                    const html = await response.text();
                    repliesList.innerHTML = html;
                    repliesList.style.display = "block";
                    button.innerHTML = `Hide replies <i class="bi bi-chevron-up"></i>`;
                } else {
                    repliesList.innerHTML = "<p class='text-danger'>Failed to load replies.</p>";
                    repliesList.style.display = "block";
                }
            } else {
                repliesList.style.display = "none";
                button.innerHTML = `Show all replies <i class="bi bi-chevron-down"></i>`;
            }
        });
    });

    // Handle reply form submissions
    document.querySelectorAll(".reply-form").forEach(form => {
        form.addEventListener("submit", async e => {
            e.preventDefault();
            const reviewId = form.getAttribute("data-review-id");
            const content = form.querySelector("textarea[name='content']").value.trim();
            if (!content) return alert("Reply content cannot be empty.");

            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

            const response = await fetch("/Games/PostReply", {
                method: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded",
                    "RequestVerificationToken": token
                },
                body: new URLSearchParams({ reviewId, content })
            });

            if (response.ok) {
                const replyHtml = await response.text();
                const repliesList = document.getElementById(`replies-list-${reviewId}`);
                repliesList.style.display = "block";
                repliesList.insertAdjacentHTML("beforeend", replyHtml);
                form.querySelector("textarea[name='content']").value = "";
            } else {
                alert("Failed to post reply.");
            }
        });
    });
});

