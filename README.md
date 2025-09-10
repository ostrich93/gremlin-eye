# Gremlin-Eye

## About

A Fullstack application that lets users manage their backlog of games, as well as read and write reviews of games. It is essentially a clone of [Backloggd](https://backloggd.com) and is currently live and accessible at https://gremlin-eye.com

## Technology

This project consists of a Backend API and a front-end SPA website, all hosted using Amazon Web Services

### Backend
- ASP.NET Core
- Entity Framework Core
- IGDB API via [igdb-dotnet](https://github.com/kamranayub/igdb-dotnet)
- Microsoft SQL Server Database
- Docker
- MimeKit

### Frontend
- React (HTML, Javascript, CSS)
- Vite
- React-Router
- React-Bootstrap
- React-Chart
- React-Modal
- Tanstack/React-Query
- FontAwesome
- Chart.js
- Axios

### AWS
- Relational Database Server
- Elastic Container Registry to store the docker images when the code pipeline kicks off
- Elastic Container Services to host containers running the API
- Elastic Load Balancer (ALB with Listener rules)
- Route53 for domain name and routing
- VPC (Subnets, NAT Gateways, Internet Gateway)
- Secrets Manager
- Systems Parameter Store
- S3 to host the static website
- Cloudfront
- Code Pipeline
	- CodeBuild
- Simple Email Service for Password reset feature
- Cloud Watch
- Cloud Formation

## Features

### As an unregistered user:
- Search for games in the search bar
- View paginated lists of games, with with the ability to filter by genre, platform, and/or average rating by users, and be able to sort the results.
- View information for individual games, with information such as the companies involved, genres, platforms, release date, stats for the play status of registered users, average ratings, and reviews
- View pages for individual reviews, which include comments
- View the user's collection of games with the ability to filter by both game information and by the completion status of the user
- View user profiles
- Register as a new user

### As a registered user:
- Add a game to my log collection
	- Mark a game as having played it (with subcategories such as completed, retired, etc.), currently playing it, is in the backlog, or is on the wishlist
- Give a numerical rating to a game
- Write and submit a review for a game
- Delete a game log entry (including reiview(s))
- Request for a password reset if I forgot my password

### As an admin user:
- Import data from IGDB in large batches for games, companies, genres, platforms, and series

## Roadmap
- Add a changelog page
- Add a dedicated search page accessible when the button the search window is clicked
- Allow users to create custom lists:
	- These lists can be ordered as the user likes
	- Can be ranked or unranked lists
	- List entries can have commentary
	- Privacy settings
- Allow users to set an avatar. This will require a way to store the images
- Allow users to reset their passwords
- Allow users to have friends lists
- Allow users to add journal entries to their game logs, with dates, time played, etc.
- Allow users to have multiple playthroughs and/or reviews for each game.
- Add styling rules to accomodate additional resolutions, including mobile views.
- Add a unique logo
- Use a more fitting palette for the UI
- Improve query performance in the backend and general performance in the frontend.
- Allow HTML styling in user reviews