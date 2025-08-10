document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".vote-form").forEach(form => {
        form.addEventListener("submit", async e => {
            e.preventDefault();

            const formData = new FormData(form);
            const reviewId = formData.get("id");
            const url = form.action;
            const token = form.querySelector('input[name="__RequestVerificationToken"]').value;

            const response = await fetch(url, {
                method: "POST",
                headers: {
                    "RequestVerificationToken": token,
                    "Content-Type": "application/x-www-form-urlencoded"
                },
                body: new URLSearchParams({ id: reviewId })
            });

            if (response.ok) {
                const data = await response.json();

                
                const reviewId = form.querySelector('input[name="id"]').value;
                const container = form.closest(".list-group-item");

                const upvoteBtn = container.querySelector(".btn-success");
                const downvoteBtn = container.querySelector(".btn-danger");

                if (upvoteBtn) {
                    upvoteBtn.innerHTML = `<i class="bi bi-hand-thumbs-up-fill"></i> ${data.upvotes}`;
                    if (data.userVote === "upvote") {
                        upvoteBtn.classList.add("active");
                        downvoteBtn.classList.remove("active");
                    } else {
                        upvoteBtn.classList.remove("active");
                    }
                }
                if (downvoteBtn) {
                    downvoteBtn.innerHTML = `<i class="bi bi-hand-thumbs-down-fill"></i> ${data.downvotes}`;
                    if (data.userVote === "downvote") {
                        downvoteBtn.classList.add("active");
                        upvoteBtn.classList.remove("active");
                    } else {
                        downvoteBtn.classList.remove("active");
                    }
                }
            } else {
                alert("Failed to submit vote.");
            }

        });
    });
});


