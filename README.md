# Span.Culturio

Monolithic application for managing cultural institutions, subscription pages, and user visits.

## Overview

This repository contains the implementation of Task 1 for the course project _ARDPSPM (Span @ FER)_.
The goal of Task 1 is to build the initial version of the **Culturio platform** as a **monolithic .NET application** that exposes a REST API for cultural institutions, users, packages, and subscriptions.

Future tasks will expand the functionality; this README will be updated as each new task is delivered.

## Scope of Task 1

Task 1 includes the following requirements:

1. Create a new solution named **Span.Culturio**.

2. Create a project **Span.Culturio.Api** inside the solution.

3. Create a SQL database named **Span.Culturio**.

4. Implement REST API methods exactly as defined in the provided OpenAPI file.

5. Connect all implemented endpoints to the database for data retrieval and persistence.

6. Implement ```/auth/register``` and ```/auth/login``` as placeholder endpoints returning ```200 OK```.

7. Deliver the solution through a publicly accessible **Azure DevOps repository**.

## API Summary

The API follows the specification provided in the OpenAPI file and includes:

### Auth

- ```POST /auth/register``` — Placeholder for user registration.

- ```POST /auth/login``` — Placeholder for user login.

### Users

- ```GET /users``` — Paginated user listing.

- ```GET /users/{id}``` — Retrieve a single user.

### Culture Objects

- ```POST /culture-objects``` — Create a cultural institution.

- ```GET /culture-objects``` — Retrieve all institutions.

- ```GET /culture-objects/{id}``` — Retrieve details by ID.

### Subscriptions

- ```POST /subscriptions``` — Create a subscription.

- ```GET /subscriptions``` — List subscriptions (supports optional userId).

- ```POST /subscriptions/track-visit``` — Record a visit.

- ```POST /subscriptions/activate``` — Activate a subscription.

### Packages

- ```GET /packages``` — Retrieve subscription packages.

## Project Status

Task 1 is the only implemented part so far.
Additional tasks will be added progressively during the semester, and the README will be updated accordingly.
