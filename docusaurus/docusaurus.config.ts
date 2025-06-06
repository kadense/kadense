import {themes as prismThemes} from 'prism-react-renderer';
import type {Config} from '@docusaurus/types';
import type * as Preset from '@docusaurus/preset-classic';

// This runs in Node.js - Don't use client-side code here (browser APIs, JSX...)

const config: Config = {
  title: 'Kadense Limited',
  tagline: 'Specialist IT Consultancy and Software Development',
  favicon: 'img/favicon.ico',
  markdown: {
    mermaid: true,
  },
  themes: [
    '@docusaurus/theme-mermaid',
  ],

  // Set the production url of your site here
  url: 'https://kadense.io/',
  // Set the /<baseUrl>/ pathname under which your site is served
  // For GitHub pages deployment, it is often '/<projectName>/'
  baseUrl: '/',

  // GitHub pages deployment config.
  // If you aren't using GitHub pages, you don't need these.
  organizationName: 'kadense', // Usually your GitHub org/user name.
  projectName: 'kadense', // Usually your repo name.

  onBrokenLinks: 'throw',
  onBrokenMarkdownLinks: 'warn',

  // Even if you don't use internationalization, you can use this field to set
  // useful metadata like html lang. For example, if your site is Chinese, you
  // may want to replace "en" with "zh-Hans".
  i18n: {
    defaultLocale: 'en',
    locales: ['en'],
  },

  presets: [
    [
      'classic',
      {
        docs: {
          sidebarPath: './sidebars.ts',
        },
        blog: {
          showReadingTime: true,
          feedOptions: {
            type: ['rss', 'atom'],
            xslt: true,
          },
          // Useful options to enforce blogging best practices
          onInlineTags: 'warn',
          onInlineAuthors: 'warn',
          onUntruncatedBlogPosts: 'warn',
        },
        theme: {
          customCss: './src/css/custom.css',
        },
      } satisfies Preset.Options,
    ],
  ],

  themeConfig: {
    image: 'img/kadense-social-card.png',
    colorMode: {
      defaultMode: 'light',
      disableSwitch: false,
      respectPrefersColorScheme: false,
    },
    navbar: {
      title: 'Kadense',
      logo: {
        alt: 'Kadense Logo',
        src: 'img/kadense-icon.png',
      },
      items: [
        {
          type: 'dropdown',
          label: 'Products', 
          position: 'left',
          items: [
            {
              label: 'The Kadense Framework',
              href: '/docs/intro',
            },
            {
              label: 'Jupyternetes',
              href: '/docs/Products/Jupyternetes/Introduction',
            },
          ]
        },
        {
          type: 'dropdown',
          label: 'Services', 
          position: 'left',
          items: [
            {
              label: 'Consultancy Services',
              href: '/consultancy-services',
            },
            {
              label: 'Infrastructure Review',
              href: '/infrastructure-review',
            },
            {
              label: 'Software Development',
              href: '/software-development',
            },
            {
              label: 'Open Source Software',
              href: '/open-source',
            },
          ]
        },
        {
          to: '/about-us', 
          label: 'About us', 
          position: 'left'
        },
        {
          to: '/blog', 
          label: 'Blog', 
          position: 'right'
        },
        {
          href: 'https://github.com/kadense/kadense',
          label: 'GitHub',
          position: 'right',
        },
      ],
    },
    footer: {
      style: 'dark',
      links: [
        {
          title: 'Services',
          items: [
            {
              label: 'Consultancy Services',
              to: '/consultancy-services',
            },
            {
              label: 'Infrastructure Review',
              to: '/infrastructure-review',
            },
            {
              label: 'Software Development',
              to: '/software-development',
            },
            {
              label: 'Open Source Software',
              to: '/open-source',
            },
          ],
        },
        {
          title: 'Products',
          items: [
            {
              label: 'The Kadense Framework',
              to: '/docs/category/the-kadence-framework',
            },
            {
              label: 'Jupyternetes',
              to: '/docs/Products/Jupyternetes/Introduction',
            },
          ],
        },
        {
          title: 'Community',
          items: [
            {
              label: 'GitHub Issues',
              href: 'https://github.com/kadense/kadense/issues',
            },
            {
              label: 'Ideas',
              href: 'https://github.com/orgs/kadense/discussions/categories/ideas',
            },
            {
              label: 'Blog',
              to: '/blog',
            },
            {
              label: 'GitHub',
              href: 'https://github.com/kadense/kadense/',
            },
          ],
        }
      ],
      copyright: `Copyright © ${new Date()}. Kadense Limited. Built with Docusaurus.`,
    },
    prism: {
      theme: prismThemes.github,
      darkTheme: prismThemes.dracula,
      additionalLanguages: ['yaml', 'json', 'csharp'],
    },
  } satisfies Preset.ThemeConfig,
};

export default config;
