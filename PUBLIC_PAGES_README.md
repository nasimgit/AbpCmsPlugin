# Public Pages Feature

This document describes the public pages feature that allows users to view published CMS pages without requiring authentication.

## Overview

The public pages feature provides a way for anonymous users to browse and view published CMS pages. This is useful for creating public-facing websites, blogs, or documentation sites.

## Features

### Backend (API)
- **PublicPageAppService**: Service that provides public access to published pages
- **AllowAnonymous**: All public page endpoints are accessible without authentication
- **Published Pages Only**: Only pages marked as published (`IsPublished = true`) are accessible
- **Home Page Support**: Special support for home pages (`IsHomePage = true`)

### Frontend (Angular)
- **Public Module**: Dedicated module for public pages
- **Public Home**: Landing page that shows the home page (if set) and recent pages
- **Page List**: Browse all published pages with search and pagination
- **Page View**: View individual pages with proper formatting

## API Endpoints

The following endpoints are available without authentication:

- `GET /api/app/public-page/home-page` - Get the home page
- `GET /api/app/public-page/published-pages` - Get list of published pages
- `GET /api/app/public-page/by-slug/{slug}` - Get a specific page by slug
- `GET /api/app/public-page/active-categories` - Get active categories
- `GET /api/app/public-page/category-by-slug/{slug}` - Get category by slug

## Frontend Routes

- `/public` - Public home page
- `/public/pages` - List of all published pages
- `/public/page/{slug}` - View a specific page

## Navigation Menu

The application includes a dynamic navigation menu with a completely flat structure that automatically updates when pages are published or unpublished:

- **All Pages** - Link to the page list view (always appears first)
- **Page 1 Title** - Direct link to individual page
- **Page 2 Title** - Direct link to individual page
- **...** - All other published pages as top-level menu items

**Menu Structure**: All published pages appear as individual top-level menu items, creating a completely flat navigation structure for easy access. There is no parent "Public Pages" menu - all pages are directly accessible from the main navigation.

The menu is automatically refreshed when:
- Pages are published or unpublished from the CMS admin
- The public home page is visited
- The application starts up

## Usage

### For Administrators

1. **Create Pages**: Use the CMS admin interface to create pages
2. **Publish Pages**: Mark pages as published using the publish button
3. **Set Home Page**: Optionally set a page as the home page
4. **Manage Content**: Use the rich text editor to create content

### For Public Users

1. **Access Public Pages**: Navigate to `/public` or click "View Public Pages" from the home page
2. **Browse Pages**: Use the page list to browse all published content
3. **Search**: Use the search functionality to find specific content
4. **View Pages**: Click on any page to view its full content

## Security

- Only published pages are accessible to anonymous users
- Unpublished pages remain private and require authentication
- All public endpoints are properly secured with `[AllowAnonymous]` attribute
- No sensitive information is exposed through public endpoints

## Styling

The public pages use Bootstrap classes and custom SCSS for styling:
- Responsive design that works on all devices
- Clean, modern appearance
- Proper typography and spacing
- Hover effects and transitions

## Content Formatting

Pages support:
- Rich text content (HTML)
- Markdown content
- Images and media
- Tables and lists
- Code blocks
- Meta information (title, description, keywords)

## Navigation

- Breadcrumb navigation for easy navigation
- Back to home and view all pages buttons
- Responsive navigation that works on mobile devices

## Future Enhancements

Potential improvements could include:
- Category-based filtering
- Tag-based filtering
- RSS feeds
- Social sharing
- Comments system
- Search engine optimization (SEO) improvements
